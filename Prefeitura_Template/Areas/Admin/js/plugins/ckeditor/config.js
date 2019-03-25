/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here.
    // For the complete reference:
    // http://docs.ckeditor.com/#!/api/CKEDITOR.config

    // The toolbar groups arrangement, optimized for two toolbar rows.
    config.allowedContent = true;
    config.toolbarGroups = [
        { name: 'clipboard', groups: ['clipboard', 'undo']}, 
        { name: 'editing',groups: ['find', 'selection', 'spellchecker']}, 
        { name: 'links' }, 
        { name: 'insert', groups: ['mediaembed']}, 
        { name: 'forms' }, 
        { name: 'tools' }, 
        { name: 'document', groups: ['mode', 'document', 'doctools']}, 
        { name: 'others' }, 
        '/', 
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup']}, 
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align']}, 
        { name: 'styles'}, 
        { name: 'colors'}, 
        { name: 'about'}    
    ];
    config.removeButtons = 'Underline,Subscript,Superscript,Tables';
    config.extraPlugins = 'mediaembed';
    config.entities = false;
    config.basicEntities = false;
    config.entities_greek = false;
    config.entities_latin = false;
    config.enterMode = CKEDITOR.ENTER_P;
};