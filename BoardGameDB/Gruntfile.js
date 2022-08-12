module.exports = function (grunt) {
    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        copy: {
            bootstrap_icons: {
                expand: true,
                cwd: 'node_modules/bootstrap-icons/font/',                
                src: '**/*.*',
                dest: 'wwwroot/css/',
            },
        },
    });

    // Load plugins
    grunt.loadNpmTasks('grunt-contrib-copy');

    // Default task(s).
    grunt.registerTask('default', ['copy']);
};
