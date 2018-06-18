module.exports = {
    plugins: [
        require("postcss-import"),
        require('postcss-cssnext')({
            browsers: ['last 2 version'],
            features: {
                customProperties: {
                    variables: {
                        '--color-white': '#fff',
                        '--color-contrast': '#ed7872',
                        '--color-red': '#dd0a2d',
                        '--color-dark-red': '#c00927',
                        '--color-black': '#000',
                        '--color-light-gray': '#ccc',
                        '--color-light-gray-2': '#eee',
                        '--color-light-gray-3': '#c7c7c7',
                        '--color-light-gray-4': '#bbb',
                        '--text-color-light': '#b6b6b6',
                        '--text-color-dark': '#555',
                        '--color-submit': '#261d43',
                        '--color-submit-hover': '#5c5573',
                        '--header-bg': '#5776f9',
                        '--font-custom': 'muli, Arial, Helvetica, sans-serif',
                        '--font-general': 'muli, Arial, Helvetica, sans-serif'
                    }
                },
                customMedia: {
                    extensions: {
                        '--for-phone-only': ' (width <= 599px)',
                        '--for-tablet-portrait-up': ' (width >= 600px)',
                        '--for-tablet-portrait-down': ' (width < 900px)',
                        '--for-tablet-landscape-up': ' (width >= 900px)',
                        '--for-desktop-down': '(width <= 1200px)',
                        '--for-desktop-up': '(width >= 1200px)',
                        '--for-big-desktop-up': '(width >= 1800px)'
                    }
                }
            }
        }),
        require('precss')
    ]
};
