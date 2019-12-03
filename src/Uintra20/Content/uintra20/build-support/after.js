const fs = require('fs');
const path = require('path')

const destPath = path.resolve(__dirname, "../../../Views/Index.cshtml");
const srcPath = path.resolve(__dirname, "../../../wwwroot/index.html");

const file = fs.readFileSync(srcPath);

fs.writeFileSync(destPath, file.toString());

const pathToAngularIndex = path.resolve(__dirname, "../src/index.html");
const pathToAngularIndexBackup = path.resolve(__dirname, "../src/_index.html");

fs.renameSync(pathToAngularIndexBackup, pathToAngularIndex);