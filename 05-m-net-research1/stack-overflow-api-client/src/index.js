import lowdb from 'lowdb';
import axios from 'axios';
import FileSync from 'lowdb/adapters/FileSync';
import fs from 'fs';

const adapter = new FileSync('db.json');
const db = lowdb(adapter);

// Set some defaults (required if your JSON file is empty)
db.defaults({questions: []})
    .write();


// Links for different StackExchange platforms
const projectManagementLink = (page) => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=agile&site=pm&pagesize=100&page=${page}&key=gxJS*Wle6xPIyfNunHxJFg((`;
const softwareEngineeeringLink = (page) => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=agile&site=softwareengineering&pagesize=100&page=${page}&key=gxJS*Wle6xPIyfNunHxJFg((`;
const projectManagementLinkScrumTag = (page) => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=scrum&site=pm&pagesize=100&page=${page}&key=gxJS*Wle6xPIyfNunHxJFg((`;
const softwareEngineeeringLinkScrumTag = (page) => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=scrum&site=softwareengineering&pagesize=100&page=${page}&key=gxJS*Wle6xPIyfNunHxJFg((`;

(async () => {
    const getData = async (link, page, exclusiveTags = []) => {
        // Throttle requests because we're nice people.
        await new Promise(resolve => setTimeout(() => resolve(), 400));

        const data = await axios.get(
            link(page)
        );
        data.data.items.map(value => {
            // If tags filtered with only exclusive tags is > 0, means that the entire question should be ignored
            if (exclusiveTags.length > 0
                &&
                value.tags.filter(
                    r => exclusiveTags[exclusiveTags.findIndex(q => q === r)] !== -1
                ).length > 0
            ) {
                console.log("found invalid question");
                // Return value is not used so this is fine for .map
                return;
            }
            db.get('questions')
                .push(value)
                .write();
        });
        if (data.data.items.length === 100 && data.data.quota_remaining > 0) {
            page++;
            console.log("Getting more data.. " + link);
            return getData(link, page);
        } else if (data.data.quota_remaining === 0) {
            console.log("Out of quota!");
        } else {
            console.log("Scraped all questions tagged 'Agile'!");
        }
    };

    await Promise.all([
        getData(projectManagementLink, 1, []),
        getData(softwareEngineeeringLink, 1, []),
        getData(softwareEngineeeringLinkScrumTag, 1, ["agile"]),
        getData(projectManagementLinkScrumTag, 1, ["agile"]),
    ]);

    const arr = db.get('questions')
        .value();
    console.log(arr.length);
    let str = ``;
    arr.map(val => {
        const ridx = Math.floor(Math.random() * Math.floor(val.tags.length));
        // Divider = ψ
        // psi, greek letter
        str = str + `${val.question_id}ψ${val.title}ψ${val.tags[ridx]}-${val.question_id}\n`;
    });

    fs.writeFileSync('./ml-data.txt', str, "utf8");
    console.log('done');
})();