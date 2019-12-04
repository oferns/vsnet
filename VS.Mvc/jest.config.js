const { pathsToModuleNameMapper } = require('ts-jest/utils');
const { compilerOptions } = require('./tsconfig.json');

module.exports = {
    preset: 'ts-jest',
    moduleNameMapper: pathsToModuleNameMapper(compilerOptions.paths),
    moduleFileExtensions: [
        "js",
        "jsx",
        "ts",
        "tsx"
    ]
    //globals: {
    //    'ts-jest': {
    //        diagnostics: {
    //            pathRegex: /\.(spec|test)\.ts$/
    //        }
    //    }
    //}
}