const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const bundleOutputDir = './wwwroot/dist';
const webpack = require('webpack');

module.exports = (env) => {
    return [{
        stats: { modules: false },
        entry: {
            'main': './src/app.tsx',
            'djayEnglish': './scss/djayEnglish.scss',
            'bootstrap': ['bootstrap/dist/css/bootstrap.min.css', 'bootstrap/dist/js/bootstrap.bundle.min.js'],
            'jquery': 'jquery/dist/jquery.js',
            'jqueryValidation': 'jquery-validation/dist/jquery.validate.min.js',
            'jqueryValidationUnobtrusive': 'jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js',
            // 'react': 'react/dist/react.js',
            // 'react-dom': 'react-dom/dist/react-dom.js',
        },
        output: {
            path: path.join(__dirname, bundleOutputDir),
            filename: '[name].js',
            library: '[name]_[hash]',
        },
        resolve: {
            extensions: ['.js', '.jsx', '.ts', '.tsx', '.html'],
            alias: {
                "app": path.join(__dirname, "src"),
            }
        },
        module: {
            rules: [
                {
                    test: /\.(ts|tsx)?$/,
                    use: 'awesome-typescript-loader?silent=true'
                },
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
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': JSON.stringify(process.env.NODE_ENV),
            }),
        ]
    }];
};