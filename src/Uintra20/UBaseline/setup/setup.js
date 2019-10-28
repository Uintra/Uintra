const exec = require('child_process').execSync;
const path = require('path');
const fs = require('fs');
const fsx = require('fs-extra');
const mkdirp = require('mkdirp');
const rimraf = require('rimraf');

class Setup {
    constructor()
    {
        this.tools = path.resolve(__dirname, 'template');
        this.project = path.resolve(__dirname, '../../Content/project');
        this.content = path.resolve(__dirname, '../../Content');
        this.workspace = path.resolve(__dirname, '../../Content/workspace');
        this.ubaslineRoot = path.resolve(__dirname, '..');
        
        this.run();
    }
    
    run()
    {
        this.prepare(() => {
            console.log('\x1b[32m', 'Setup UBasline frontend project'); 
            rimraf.sync(path.normalize(`${this.workspace}/src`));
            this.copyNgProject();
            this.copyTools();

            console.log('\x1b[32m', 'Copy project overrides');
            this.copyOverrides();
            
            console.log('\x1b[32m', 'Npm install...', '\x1b[0m');
            exec(`cd ${this.workspace} && npm i`);
            exec(`cd ${this.content} && npm i`);

            console.log('\x1b[32m', 'Done.', '\x1b[0m');
            console.log('\x1b[36m', `For DEVELOPMENT open folder "${this.content}" and run 'dev.cmd'`, '\x1b[0m');
            console.log('\x1b[36m', `For BUILD navigate to "${this.content}" and run 'npm run build'`, '\x1b[0m');
            console.log('\x1b[36m', `start "${this.content}"`, '\x1b[0m');
        })
    }

    prepare(cb)
    {
        mkdirp(this.project, err => {
            mkdirp(this.workspace, err => {
                if (!err) cb();
            })
        })
    }
    
    copyTools()
    {
        fs.readdirSync(this.tools).forEach(file => {
            fs.copyFileSync(path.normalize(`${this.tools}/${file}`), path.normalize(`${this.content}/${file}`));
        })
    }
    copyNgProject()
    {
        this.copy(this.ubaslineRoot, this.workspace);
    }

    copyOverrides(){
        this.copy(this.project, this.workspace);
    }

    copy(fromPath, toPath){
        const names = this.getSources(fromPath);

        names.forEach(file => {
            try {
                fs.copyFileSync(file.src, path.normalize(`${toPath}/${file.name}`));
            } catch (err) {
                // probably directory
                fsx.copySync(file.src, path.normalize(`${toPath}/${file.name}`));
            }
        })
    }

    getSources(folderPath)
    {
        return fs.readdirSync(folderPath)
            .filter(name => {
                const res = name !== 'setup' && name !== 'node_modules';
                return res;
            })
            .map(name => {
                let src = path.normalize(`${folderPath}/${name}`);
                return {name, src}
            });
    }
}

new Setup;