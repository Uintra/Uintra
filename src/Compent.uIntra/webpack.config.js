'use strict';
const path = require('path');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const webpack = require("webpack");

module.exports = (env) => {
    const isDev = !(env && env.prod);

    let config = {
        entry: {
            vendors:['jquery', './Content/vendors'],
            head_css: './Content/head_css.js',
            app: './Content/main.js'
        },
        output: {
            path: path.join(__dirname, 'build'),
            filename: '[name].js'
        },
        module: {
            loaders: [
                {
                    test: /\.js$/,
                    exclude: /(node_modules|bower_components)/,
                    loader: 'babel-loader',
                    query: {
                        presets: ["env"]
                    }
                },
                {
                    test: /\.css$/,
                    use: ExtractTextPlugin.extract([
                        isDev ? 'css-loader?sourceMap' : 'css-loader?minimize',
                        'postcss-loader'
                    ])
                },
                {
                    test: /\.(png|svg|gif)$/,
                    loader: 'url-loader?limit=100000'
                },
                {
                    test: /\.(woff|woff2|eot|ttf)$/,
                    loader: 'file-loader'
                }
            ]
        },
        plugins: [
            new webpack.NoEmitOnErrorsPlugin(),
            new ExtractTextPlugin('[name].css'),
            new webpack.optimize.CommonsChunkPlugin({
                names: ['app', 'vendors'],
                minChunks: Infinity
            }),
            new webpack.ProvidePlugin({
                $: 'jquery',
                jQuery: 'jquery'
            })
        ],
        stats: {
            children: false,
            colors: true
        }
    }

    if (isDev) {
        // Plugins that apply in development builds only
        config.devtool = 'source-map'
    }
    else {
        // Plugins that apply in production builds only
        config.plugins.push(new webpack.optimize.UglifyJsPlugin())

        //Many libraries will key off the process.env.NODE_ENV variable to determine what should be included in the library. 
        config.plugins.push(new webpack.DefinePlugin({
            'process.env': {
                'NODE_ENV': JSON.stringify('production')
            }
        }))
    }

    return config;
}