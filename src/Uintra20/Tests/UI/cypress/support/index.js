// ***********************************************************
// This example support/index.js is processed and
// loaded automatically before your test files.
//
// This is a great place to put global configuration and
// behavior that modifies Cypress.
//
// You can change the location of this file or turn off
// automatically serving support files with the
// 'supportFile' configuration option.
//
// You can read more here:
// https://on.cypress.io/configuration
// ***********************************************************

// Import commands.js using ES2015 syntax:
import './commands'

require('cypress-xpath')
import "lorem-ipsum"
import 'cypress-file-upload'
import '../support/centralFeedCommands.js'
import '../support/socialActivityCommands.js'
import '../support/loremText.js'
import '../support/newsActivityCommands.js'
import '../support/eventActivityCommands.js'
import '../support/loginCommands.js'




// Alternatively you can use CommonJS syntax:
// require('./commands')
