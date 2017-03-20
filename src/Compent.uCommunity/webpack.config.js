var Path = require('path');
//var WebpackNotifierPlugin = require('webpack-notifier');

module.exports = {
    entry: {
        css: './Content/maincss.js',
        main: './Content/main.js'
    },
    output: {
        path: Path.join(__dirname, 'build'),
        filename: '[name].js',
        //library: 'App',
        //libraryTarget: 'this'
    },
    resolve: {
        extensions: ['.Webpack.js', '.js'],
        //modulesDirectories: ['node_modules'],
    },
    devtool: 'source-map',
    module: {
        loaders: [
            {
                test: /\.js$/,
                exclude: /(node_modules|bower_components)/,
                loader: 'babel-loader',
                query: {
                    presets: ['es2015']
                }
            },
            {
                test: /\.css$/,
                loader: "style-loader!css-loader!resolve-url-loader!postcss-loader"
            },
            {
                test: /\.(png|woff|woff2|eot|ttf|svg|gif)$/,
                loader: 'url-loader?limit=100000'
            }
        ]
    },
    plugins: [
    //    new WebpackNotifierPlugin()
    ]
};
