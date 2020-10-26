Cypress.Commands.add('lorem', (filePath) => {
    const LoremIpsum = require("lorem-ipsum").LoremIpsum;
    return cy.wrap(new LoremIpsum({
        sentencesPerParagraph: {
            max: 8,
            min: 4
        },
        wordsPerSentence: {
            max: 16,
            min: 4
        }
    })) 
})