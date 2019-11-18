const { watch } = require('gulp');
const gulp = require('gulp');
const exec = require('child_process').execSync;
const fs = require('fs');
const path = require('path');
const fsx = require('fs-extra');
const mkdirp = require('mkdirp');
const rimraf = require("rimraf");

class Tool {
    constructor()
    {
        this.project = path.resolve(__dirname, './project');
        this.content = path.resolve(__dirname, '.');
        this.workspace = path.resolve(__dirname, './workspace');
        this.build = path.resolve(__dirname, './workspace/dist/BaseLine');
        this.wwwroot = path.resolve(__dirname, '../wwwroot');
        this.prepare();
    }

    prepare()
    {
        this.copyOverrides();
    }

    copyOverrides()
    {
        const names = this.getOverrides();

        names.forEach(file => {
            try {
                fs.copyFileSync(file.src, path.normalize(`${this.workspace}/${file.name}`));
            } catch (err) {
                // probably directory
                fsx.copySync(file.src, path.normalize(`${this.workspace}/${file.name}`));
            }
        })
    }

    getOverrides()
    {
        return fs.readdirSync(this.project)

            .map(name => {
                let src = path.normalize(`${this.project}/${name}`);
                return {name, src}
            });
    }

    comparator(stream, file, dest)
    {
        return changed.compareLastModifiedTime(stream, file, dest)
    }

    onChanged(path)
    {
        console.log(`changed ${path}`);
        fsx.copy(path.replace('workspace', './workspace'), path.replace('workspace', './project'));
    }

    dev()
    {
        watch([`./workspace/**/*`]).on('change', (file) => this.onChanged(file));
    }

    buildProd2(done)
    {
        this.prepare();
        console.log('Npm install...');
        exec(`cd ${this.workspace} && npm install`);

        console.log('Building...');
        rimraf.sync("../wwwroot");

        exec(`cd ${this.workspace} && ${this.workspace}/node_modules/.bin/ng build --named-chunks --prod --deploy-url /wwwroot/`);

        mkdirp('../wwwroot', err => {
            this.getBuildedFileNames().forEach(file => {
                try {
                    fs.copyFileSync(file.src, path.normalize(`${this.wwwroot}/${file.name}`));
                } catch (err) {
                    // probably directory
                    fsx.copySync(file.src, path.normalize(`${this.wwwroot}/${file.name}`));
                }
            });

            this.writeIndex();

            done();
        });
    }


    getBuildedFileNames()
    {
        return fs.readdirSync(this.build)
            .map(name => {
                let src = path.normalize(`${this.build}/${name}`);
                return {name, src}
            });
    }

    writeIndex()
    {
      let metaTags = `
            @if (Model.MetaData != null)
            {
              <meta name="title" content="@Model.MetaData.MetaTitle">
              <meta name="description" content="@Model.MetaData.MetaDescription">
              <meta property="og:title" content="@Model.MetaData.SocialTitle">
              <meta property="og:description" content="@Model.MetaData.SocialDescription">
              if (Model.MetaData.SocialImageUrl.HasValue())
              {
                <meta property="og:image" content="@Model.MetaData.SocialImageUrl">
              }
              <link rel="canonical" href="@Model.MetaData.CanonicalUrl" />
            }`;

        let angularIndex = fs.readFileSync('../wwwroot/index.html', {encoding: 'utf-8'});
        let replaced = angularIndex.replace('<!-- head-scripts -->', '@Html.Raw(Model.HeaderScript)');
        replaced = replaced.replace('<!-- meta-data -->', metaTags);
        replaced = replaced.replace('<!-- site-title -->', '<title>@Model.Title</title>');
        replaced = replaced.replace('<!-- head-local-scripts -->', '@Html.Raw(Model.HeaderLocalScript)');
        replaced = replaced.replace('<!-- body-local-scripts -->', '@Html.Raw(Model.BodyLocalScript)');
        replaced = replaced.replace('<!-- footer-local-scripts -->', '@Html.Raw(Model.FooterLocalScript)');
        replaced = replaced.replace('<!-- after-body-start-scripts -->', '@Html.Raw(Model.BodyScript)');
        replaced = replaced.replace('<!-- before-body-end-scripts -->', '@Html.Raw(Model.FooterScript)');
        replaced = replaced.replace('<!-- support -->', '@using Compent.Shared.Extensions.Bcl @model UBaseline.Core.IndexPage.IndexPageModel');

        fs.writeFile("../Views/Index.cshtml", replaced, function(err) {
            if (err) return console.log(err);

            console.log("Index.cshtml updated.");
        });
    }
}

const task = new Tool();

exports.default = () => task.dev();
exports.dev = () => task.dev();
exports.build = (done) => task.buildProd2(done);
