var Path = require('path');

module.exports = {
    entry: {
        css: './Content/maincss.js',
        main: './Content/main.js'
    },
    output: {
        path: Path.join(__dirname, 'build'),
        filename: '[name].js'
    },
    resolve: {
        extensions: ['.Webpack.js', '.js']
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
    }
};
