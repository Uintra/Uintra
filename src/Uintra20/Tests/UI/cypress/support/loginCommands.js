Cypress.Commands.add('login', ({ email, password }) => {

    cy.fixture('pagesElements.json')
        .then((p) => {
            const pagesElements = p

            cy.visit('/login');
            cy.xpath(pagesElements.loginPage[0].loginInput).type(email)
            cy.xpath(pagesElements.loginPage[0].passwordInput).type(password)
            cy.xpath(pagesElements.loginPage[0].loginButton).click().waitForBrowser
        })
})