context("News activivtu create, edit, pin/unpin test", () => {

    let pagesElements
    let validationMassages
    let userData

    describe('user create, edit, pin/unpin news', () => {
        beforeEach(() => {

            cy.fixture('userData.json')
                .then((uD) => {
                    const user = uD
                    cy.login({ email: user.email, password: user.password })
                    userData = uD
                })

            cy.fixture('pagesElements.json')
                .then((np) => pagesElements = np)

            cy.fixture('validationMassages.json')
                .then((v) => validationMassages = v);

        })

        it('Create event validation check', () => {

            /**
             * On central feed click on ceate news button
             * do not fill data
             * click on kreste news button 
             */

            cy.openCrateEventPage()


            cy.location().should((loc) => {
                expect(loc.href).contains("/create-event")
            })

            cy.scrollTo('bottom')

            cy.get(pagesElements.eventCreateEditPage[0].createButton)
                .should('be.visible')
                .should('have.length', 1)
                .click().waitForBrowser


            /**
             * Check validation mesages
             */
            cy.xpath(pagesElements.eventCreateEditPage[0].titleValidation).should('contain.text', validationMassages.createEvent[0])
            cy.xpath(pagesElements.eventCreateEditPage[0].descriptionValidation).should('contain.text', validationMassages.createEvent[1])
        })

        it('Create event test', () => {
            cy.lorem().then(lorem => {
                let title = 'AT ' + lorem.generateWords(2)
                let description = lorem.generateParagraphs(1)

                cy.openCrateEventPage()

                //check Owner selector default value
                cy.get(pagesElements.eventCreateEditPage[0].ownerSelector).should('have.text', userData.firstName + ' ' + userData.lastName)

                //fill reuired data
                cy.xpath(pagesElements.eventCreateEditPage[0].titleInput).type(title)
                cy.get(pagesElements.eventCreateEditPage[0].descriptionRTE).type(description)

                cy.submitEvent()

                //Check that data match with filled 
                cy.get(pagesElements.activityDetailsPage[0].ownerName).should('have.text', userData.firstName + ' ' + userData.lastName)
                cy.get(pagesElements.activityDetailsPage[0].activityName).should('have.text', 'Events')
                cy.get(pagesElements.activityDetailsPage[0].title).should('have.text', title)
                cy.get(pagesElements.activityDetailsPage[0].description).should('have.text', description)
            })
        })

        it.skip('Edit event test', () => {

        })

        it.skip('Pin/unpin event test', () => {

        })
    })
})