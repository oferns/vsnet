const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const RemovePlugin = require("remove-files-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");
const path = require("path");

module.exports = env => {
  return {
    mode:
      (env ? env : (env = {})) && env.MODE
        ? env.MODE
        : (env.MODE = "production"),
    optimization: {
      minimize: env.MODE === "production" || env.MODE === "staging"
    },
    entry: ["./app.ts", "./app.scss"],
    devtool: false, // We handle source maps with a plugin due to issues resolving the paths
    module: {
      rules: [
        {
          test: /\.tsx?$/,
          use: "babel-loader",
          exclude: /node_modules|bin|obj|wwwroot/
        },
        {
          test: /\.(sa|sc|c)ss$/,
          use: [
            {
              loader: MiniCssExtractPlugin.loader,
              options: {
                hmr: env.MODE === "development" // Hot module reload
              }
            },
            "css-loader",
            "sass-loader"
          ],
          exclude: /node_modules|bin|obj|wwwroot/
        },
        {
          test: /\.(html)$/,
          use: {
            loader: "html-loader",
            options: {
              attrs: false
            }
          },
          exclude: /node_modules|bin|obj|wwwroot/
        }
      ]
    },
    resolve: {
      extensions: [".js", ".ts", ".tsx", ".scss", ".sass", ".css"]
    },
    output: {
      filename: "js/app.js",
      path: path.resolve(__dirname, "wwwroot")
    },
    plugins: [
      // new CleanWebpackPlugin({
      //     exclude: [path.resolve(__dirname,"js/webcomponentsjs/**/*")],
      //    }),  // Delete everything in wwwroot except the webcomponents

      new RemovePlugin({
        before: {
            include :[path.resolve(__dirname)],
            exclude : [path.resolve(__dirname, "js/webcomponents")]
          // parameters for "before normal compilation" stage.
        },
        watch: {
          // parameters for "before watch compilation" stage.
        },
        after: {
          // parameters for "after normal and watch compilation" stage.
        }
      }),
      new webpack.SourceMapDevToolPlugin({
        // generate source maps
        filename: "[file].map",
        fallbackModuleFilenameTemplate: "[absolute-resource-path]",
        moduleFilenameTemplate: "[absolute-resource-path]"
      }),
      new MiniCssExtractPlugin({
        // extract the styles imported in app.scss into app.css
        filename: "css/app.css"
      }),
      new CopyPlugin([
        // Copy the webcomponents polyfill & any assets
        {
          from: "**/*.js",
          to: "js/webcomponentsjs",
          context: path.resolve(
            __dirname,
            "node_modules/@webcomponents/webcomponentsjs"
          )
        },
        {
          from: "**/*",
          to: "",
          context: path.resolve(__dirname, "../assets")
        }
      ])
    ]
  };
};
