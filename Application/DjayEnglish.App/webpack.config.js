const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const bundleOutputDir = './wwwroot/dist';

module.exports = (env) => {
    return [{
        stats: { modules: false },
        entry: {
            'djayEnglish': './scss/djayEnglish.scss',
            'bootstrap': ['bootstrap/dist/css/bootstrap.min.css', 'bootstrap/dist/js/bootstrap.bundle.min.js'],
            'jquery': 'jquery/dist/jquery.js',
            'jqueryValidation': 'jquery-validation/dist/jquery.validate.min.js',
            'jqueryValidationUnobtrusive': 'jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js',
        },
        output: {
            path: path.join(__dirname, bundleOutputDir),
            filename: '[name].js',
            library: '[name]_[hash]',
        },
        module: {
            rules: [
                {
                    test: /\.(scss)$/,
                    use: [{
                        loader: MiniCssExtractPlugin.loader,
                    }, {
                        loader: 'css-loader',
                    }, {
                        loader: 'postcss-loader',
                        options: {
                            postcssOptions: {
                                plugins: [
                                    [
                                        require('precss'),
                                        require('autoprefixer')

                                    ],
                                ],
                            },
                        }
                    }, {
                        loader: 'sass-loader'
                    }]
                },
                { test: /\.css$/, use: [{ loader: MiniCssExtractPlugin.loader, }, "css-loader"] },
            ]
        },
        plugins: [
            new MiniCssExtractPlugin({
                filename: "[name].css",
                chunkFilename: "[id].css"
            }),
        ]
    }];
};