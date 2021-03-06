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


const keys = [
    "gxJS*Wle6xPIyfNunHxJFg((",
    "Leo*1RuvVm5e24hPGIEByQ((",
    "u7IcmYUsDX3euw6rqiENCQ((",
    "TkBD8qEw2LLWa)*ZazEfjw((",
    "ChZNg9j6JB9ZX6fSSNzp5g((",
    "K4ckBUjQWlJJqNPVA4ffuA((",
    "9LtspeQ0sG*V9RwNaWwzFA(("
];


const stackoverflowLink_javascript = (page) => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=javascript&site=stackoverflow&pagesize=100&page=${page}&key=${keys[0]}`;
const stackoverflowLink_php = (page) => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=php&site=stackoverflow&pagesize=100&page=${page}&key=${keys[1]}`;
const stackoverflowLink_jquery = (page) => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=jquery&site=stackoverflow&pagesize=100&page=${page}&key=${keys[2]}`;
const stackoverflowLink_css = (page) => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=css&site=stackoverflow&pagesize=100&page=${page}&key=${keys[3]}`;
const stackoverflowLink_html = (page) => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=html&site=stackoverflow&pagesize=100&page=${page}&key=${keys[4]}`;
const stackoverflowLink_mysql = (page) => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=mysql&site=stackoverflow&pagesize=100&page=${page}&key=${keys[5]}`;
const stackoverflowLink_sql = (page) => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=sql&site=stackoverflow&pagesize=100&page=${page}&key=${keys[6]}`;

(async () => {
    const getData = async (link, page, exclusiveTags = []) => {
        // Throttle requests because we're nice people.
        // await new Promise(resolve => setTimeout(() => resolve(), 200));

        let data;
        try {
            data = await axios.get(
                link(page)
            );
        } catch (e) {
            console.log(e);
            console.log(link(page))
        }
        if (!data) return;

        data.data.items.map(value => {
            // If tags filtered with only exclusive tags is > 0, means that the entire question should be ignored
            if (exclusiveTags.length > 0
                &&
                value.tags.filter(
                    r => exclusiveTags[exclusiveTags.findIndex(q => q === r)] !== -1
                ).length > 0
            ) {
                // console.log("found invalid question");
                // Return value is not used so this is fine for .map
                return;
            }
            db.get('questions')
                .push(value)
                .write();
        });
        if (data.data.items.length === 100 && data.data.quota_remaining > 0) {
            page++;
            console.log(`Getting more data.. currently on page: ${page}` + link);
            return getData(link, page);
        } else if (data.data.quota_remaining === 0) {
            console.log("Out of quota!");
        } else {
            console.log("Scraped all questions!");
        }
    };
    //
    // await Promise.all([
    //     getData(stackoverflowLink_javascript, 1, []),
    //     getData(stackoverflowLink_css, 1, ["javascript"]),
    //     getData(stackoverflowLink_html, 1, ["javascript", "css"]),
    //     getData(stackoverflowLink_jquery, 1, ["javascript", "css", "html"]),
    //     getData(stackoverflowLink_mysql, 1, ["javascript", "css", "html", "jquery"]),
    //     getData(stackoverflowLink_php, 1, ["javascript", "css", "html", "jquery", "mysql"]),
    //     getData(stackoverflowLink_sql, 1, ["javascript", "css", "html", "jquery", "mysql", "php"]),
    // ]);

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