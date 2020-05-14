Cypress.Commands.add('submitNews', () => {
    cy.fixture('pagesElements.json')
        .then((p) => {
            const pagesElements = p

            cy.server()
            cy.route('POST', 'https://**/ubaseline/api/newsApi/create').as('createNews');
            cy.get(pagesElements.newsCreateEditPage[0].createButton).click()
            cy.wait('@createNews');

            cy.location().should((loc) => {
                expect(loc.href).contains("/news-details?id")
            })

        })
})