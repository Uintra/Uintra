Cypress.Commands.add('submitEvent', () => {
    cy.fixture('pagesElements.json')
        .then((p) => {
            const pagesElements = p

            cy.server()
            cy.route('POST', 'https://**/ubaseline/api/events/create').as('createEvent');
            cy.get(pagesElements.eventCreateEditPage[0].createButton).click()
            cy.wait('@createEvent');

            cy.location().should((loc) => {
                expect(loc.href).contains("/details-event?id")

            })
        })
})