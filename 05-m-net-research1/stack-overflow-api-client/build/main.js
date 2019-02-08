require('source-map-support/register')
module.exports =
/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "/";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ({

/***/ "./src/index.js":
/*!**********************!*\
  !*** ./src/index.js ***!
  \**********************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var lowdb__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! lowdb */ "lowdb");
/* harmony import */ var lowdb__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(lowdb__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var axios__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! axios */ "axios");
/* harmony import */ var axios__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(axios__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var lowdb_adapters_FileSync__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! lowdb/adapters/FileSync */ "lowdb/adapters/FileSync");
/* harmony import */ var lowdb_adapters_FileSync__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(lowdb_adapters_FileSync__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var fs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! fs */ "fs");
/* harmony import */ var fs__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(fs__WEBPACK_IMPORTED_MODULE_3__);




const adapter = new lowdb_adapters_FileSync__WEBPACK_IMPORTED_MODULE_2___default.a('db.json');
const db = lowdb__WEBPACK_IMPORTED_MODULE_0___default()(adapter); // Set some defaults (required if your JSON file is empty)

db.defaults({
  questions: []
}).write(); // Links for different StackExchange platforms

const projectManagementLink = page => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=agile&site=pm&pagesize=100&page=${page}&key=gxJS*Wle6xPIyfNunHxJFg((`;

const softwareEngineeeringLink = page => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=agile&site=softwareengineering&pagesize=100&page=${page}&key=gxJS*Wle6xPIyfNunHxJFg((`;

const projectManagementLinkScrumTag = page => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=scrum&site=pm&pagesize=100&page=${page}&key=gxJS*Wle6xPIyfNunHxJFg((`;

const softwareEngineeeringLinkScrumTag = page => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=scrum&site=softwareengineering&pagesize=100&page=${page}&key=gxJS*Wle6xPIyfNunHxJFg((`;

const keys = ["gxJS*Wle6xPIyfNunHxJFg((", "Leo*1RuvVm5e24hPGIEByQ((", "u7IcmYUsDX3euw6rqiENCQ((", "TkBD8qEw2LLWa)*ZazEfjw((", "ChZNg9j6JB9ZX6fSSNzp5g((", "K4ckBUjQWlJJqNPVA4ffuA((", "9LtspeQ0sG*V9RwNaWwzFA(("];

const stackoverflowLink_javascript = page => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=javascript&site=stackoverflow&pagesize=100&page=${page}&key=${keys[0]}`;

const stackoverflowLink_php = page => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=php&site=stackoverflow&pagesize=100&page=${page}&key=${keys[1]}`;

const stackoverflowLink_jquery = page => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=jquery&site=stackoverflow&pagesize=100&page=${page}&key=${keys[2]}`;

const stackoverflowLink_css = page => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=css&site=stackoverflow&pagesize=100&page=${page}&key=${keys[3]}`;

const stackoverflowLink_html = page => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=html&site=stackoverflow&pagesize=100&page=${page}&key=${keys[4]}`;

const stackoverflowLink_mysql = page => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=mysql&site=stackoverflow&pagesize=100&page=${page}&key=${keys[5]}`;

const stackoverflowLink_sql = page => `https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=sql&site=stackoverflow&pagesize=100&page=${page}&key=${keys[6]}`;

(async () => {
  const getData = async (link, page, exclusiveTags = []) => {
    // Throttle requests because we're nice people.
    // await new Promise(resolve => setTimeout(() => resolve(), 200));
    let data;

    try {
      data = await axios__WEBPACK_IMPORTED_MODULE_1___default.a.get(link(page));
    } catch (e) {
      console.log(e);
      console.log(link(page));
    }

    if (!data) return;
    data.data.items.map(value => {
      // If tags filtered with only exclusive tags is > 0, means that the entire question should be ignored
      if (exclusiveTags.length > 0 && value.tags.filter(r => exclusiveTags[exclusiveTags.findIndex(q => q === r)] !== -1).length > 0) {
        // console.log("found invalid question");
        // Return value is not used so this is fine for .map
        return;
      }

      db.get('questions').push(value).write();
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
  }; //
  // await Promise.all([
  //     getData(stackoverflowLink_javascript, 1, []),
  //     getData(stackoverflowLink_css, 1, ["javascript"]),
  //     getData(stackoverflowLink_html, 1, ["javascript", "css"]),
  //     getData(stackoverflowLink_jquery, 1, ["javascript", "css", "html"]),
  //     getData(stackoverflowLink_mysql, 1, ["javascript", "css", "html", "jquery"]),
  //     getData(stackoverflowLink_php, 1, ["javascript", "css", "html", "jquery", "mysql"]),
  //     getData(stackoverflowLink_sql, 1, ["javascript", "css", "html", "jquery", "mysql", "php"]),
  // ]);


  const arr = db.get('questions').value();
  console.log(arr.length);
  let str = ``;
  arr.map(val => {
    const ridx = Math.floor(Math.random() * Math.floor(val.tags.length)); // Divider = ψ
    // psi, greek letter

    str = str + `${val.question_id}ψ${val.title}ψ${val.tags[ridx]}-${val.question_id}\n`;
  });
  fs__WEBPACK_IMPORTED_MODULE_3___default.a.writeFileSync('./ml-data.txt', str, "utf8");
  console.log('done');
})();

/***/ }),

/***/ 0:
/*!****************************!*\
  !*** multi ./src/index.js ***!
  \****************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(/*! C:\gitrepos\C-sharp\05-m-net-research1\stack-overflow-api-client\src/index.js */"./src/index.js");


/***/ }),

/***/ "axios":
/*!************************!*\
  !*** external "axios" ***!
  \************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = require("axios");

/***/ }),

/***/ "fs":
/*!*********************!*\
  !*** external "fs" ***!
  \*********************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = require("fs");

/***/ }),

/***/ "lowdb":
/*!************************!*\
  !*** external "lowdb" ***!
  \************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = require("lowdb");

/***/ }),

/***/ "lowdb/adapters/FileSync":
/*!******************************************!*\
  !*** external "lowdb/adapters/FileSync" ***!
  \******************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = require("lowdb/adapters/FileSync");

/***/ })

/******/ });
//# sourceMappingURL=main.map