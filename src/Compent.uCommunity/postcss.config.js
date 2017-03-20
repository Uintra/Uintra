module.exports = {
    plugins: [
        require("postcss-import"),
        require('precss'),
        require('autoprefixer')({ browsers: ['last 2 versions'] }),
        require('postcss-css-variables')
    ]
};
