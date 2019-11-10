const webpack = require('webpack');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const CopyPlugin = require('copy-webpack-plugin');
const path = require('path');

module.exports = env => {
    return ({
        mode: (env ? env : (env = {})) && env.MODE ? env.MODE : (env.MODE = 'development'),
        entry: ['./app.ts','./app.scss'],
        // devtool: env && env.MODE === 'development' ? 'inline-source-map' : 'none',
        devtool: false,
        module: {
            rules: [
                {
                    test: /\.tsx?$/,
                    use: 'babel-loader',
                    exclude: /node_modules/
                },
                {
                    test: /\.(sa|sc|c)ss$/,
                    exclude: /node_modules/,
                    use: [
                        {
                            loader: MiniCssExtractPlugin.loader,
                            options: {
                                hmr: env.MODE  === 'development'
                            }
                        },
                        'css-loader',
                        'sass-loader'
                    ] 
                },
                {
                    test: /\.(html)$/,
                    use: {
                        loader: 'html-loader',
                        options: {
                            attrs: false,
                        },
                    },
                    exclude: /node_modules/
                },
            ],
        },
        resolve: {
            extensions: ['.tsx', '.ts', '.scss','.sass','.css']            
        },
        output: {
            filename: 'app.js',
            path: path.resolve(__dirname, 'wwwroot/js')
        },
        plugins: [
            new CleanWebpackPlugin(),          
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map',
                fallbackModuleFilenameTemplate: '[absolute-resource-path]',
                moduleFilenameTemplate: '[absolute-resource-path]'
                
            }),
            new MiniCssExtractPlugin({
                filename: "../css/app.css"
            }),
            new CopyPlugin([
                { from: 'node_modules/@webcomponents/webcomponentsjs', to: 'webcomponentsjs' }
            ])
        ]
    });
};