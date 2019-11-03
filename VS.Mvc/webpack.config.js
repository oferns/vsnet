var webpack = require('webpack');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const CopyPlugin = require('copy-webpack-plugin');
const path = require('path');

module.exports = env => {
    return ({
        mode: env && env.MODE ? env.MODE : 'development',
        entry: './app.ts',
        //devtool: env && env.MODE === 'development' ? 'inline-source-map' : 'none',
        devtool: false,
        module: {
            rules: [
                {
                    test: /\.tsx?$/,
                    use: 'babel-loader',
                    exclude: /node_modules/
                    //resolve: {
                    //    modules: [
                    //        path.resolve(__dirname, '**/*'),
                    //    ]
                    //}
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
            extensions: ['.tsx', '.ts']            
        },
        output: {
            filename: 'app.js',
            path: path.resolve(__dirname, 'wwwroot/js')
        },
        plugins: [
            new CleanWebpackPlugin(),
            new CopyPlugin([
                { from: 'node_modules/@webcomponents/webcomponentsjs', to: 'webcomponentsjs' },
            ]),
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map',
                fallbackModuleFilenameTemplate: '[absolute-resource-path]',
                moduleFilenameTemplate: '[absolute-resource-path]'
                
            })
        ]
    });
};