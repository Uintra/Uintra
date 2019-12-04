const fs = require('fs');
const path = require('path')

const pathToIndex = path.resolve(__dirname, "../../../Views/Index.cshtml");

const pathToAngularIndex = path.resolve(__dirname, "../src/index.html");
const pathToAngularIndexBackup = path.resolve(__dirname, "../src/_index.html");
fs.renameSync(pathToAngularIndex, pathToAngularIndexBackup);

const styleSheetTag = /<link[^>]*rel="[^>]*\/wwwroot\/styles[^>]*"[^>]*>/gi;
const scriptTag = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi;

const file = fs.readFileSync(pathToIndex);
const cleaned = file.toString().replace(scriptTag, '').replace(styleSheetTag, '');

fs.writeFileSync(pathToAngularIndex, cleaned);