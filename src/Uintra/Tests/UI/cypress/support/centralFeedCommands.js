Cypress.Commands.add('openSocialCreatePopup', () => {

    cy.fixture('pagesElements.json')
        .then((cf) => {
            const pagesElements = cf

            cy.get(pagesElements.centralFeedPage[0].createSocialInput)
                .click()
                .waitForBrowser

            cy.get(pagesElements.sosialPopup[0].popup).should('be.visible')
        })
})

Cypress.Commands.add('openCrateNewsPage', () => {

    cy.fixture('pagesElements.json')
        .then((cf) => {
            const pagesElements = cf
            // const currentURL = cy.url()

            // if (currentURL.should('not.eq', Cypress.config().baseUrl)) {
            //      cy.visit('/').waitForBrowser
            // }
            // else{
            cy.xpath(pagesElements.centralFeedPage[0].createNewsButton)
                .should('be.visible')
                .should('have.length', 1)
                .click()
                .waitForBrowser

            cy.url().should('contain', "/news/news-create")
            //  }
        })
})

Cypress.Commands.add('openCrateEventPage', () => {

    cy.fixture('pagesElements.json')
        .then((cf) => {
            const pagesElements = cf
            // const currentURL = cy.url()

            // if (currentURL.should('not.eq', Cypress.config().baseUrl)) {
            //      cy.visit('/').waitForBrowser
            // }
            // else{
            cy.xpath(pagesElements.centralFeedPage[0].createEventButton)
                .should('be.visible')
                .should('have.length', 1)
                .click()
                .waitForBrowser

            cy.url().should('contain', "/events/create-event")
            //  }
        })
})