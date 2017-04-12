var Webpack = require('webpack');
var Path = require('path');
var ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
    entry: {
        head_css: './Content/head_css.js',
        app: './Content/main.js'
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
                loader: ExtractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: [{
                        loader: 'css-loader',
                        options: {
                            //minimize: true
                        }
                    }, 'resolve-url-loader', 'postcss-loader']
                })
            },
            {
                test: /\.(png|woff|woff2|eot|ttf|svg|gif)$/,
                loader: 'url-loader?limit=100000'
            }
        ]
    },
    plugins: [
        new Webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery",
            "window.jQuery": "jquery"
        }),

        new ExtractTextPlugin('[name].css')
    ]
};
