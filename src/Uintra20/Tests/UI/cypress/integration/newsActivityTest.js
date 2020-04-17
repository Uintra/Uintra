context("News activivtu create test", () => {

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

        it('Create news validation check', () => {

            /**
             * On central feed click on ceate news button
             * do not fill data
             * click on kreste news button 
             */


            cy.openCrateNewsPage()

            cy.get(pagesElements.newsCreateEditPage[0].createButton)
                .click().waitForBrowser


            /**
             * Check validation mesages
             */
            cy.xpath(pagesElements.eventCreateEditPage[0].titleValidation).should('contain.text', validationMassages.createNews[0])
            cy.xpath(pagesElements.eventCreateEditPage[0].descriptionValidation).should('contain.text', validationMassages.createNews[1])


        })

        it('Create news test', () => {
            cy.lorem().then(lorem => {

                let title = 'AT ' + lorem.generateWords(2)
                let description = lorem.generateParagraphs(1)

                cy.openCrateNewsPage()

                //check Owner selector default value
                cy.get(pagesElements.newsCreateEditPage[0].ownerSelector).should('have.text', userData.firstName + ' ' + userData.lastName)

                //fill reuired data
                cy.xpath(pagesElements.newsCreateEditPage[0].titleInput).type(title)
                cy.get(pagesElements.newsCreateEditPage[0].newsDescriptionRTE).type(description)

                cy.submitNews()

                //Check that data match with filled 
                cy.get(pagesElements.activityDetailsPage[0].ownerName).should('have.text', userData.firstName + ' ' + userData.lastName)
                cy.get(pagesElements.activityDetailsPage[0].activityName).should('have.text', 'News')
                cy.get(pagesElements.activityDetailsPage[0].title).should('have.text', title)
                cy.get(pagesElements.activityDetailsPage[0].description).should('have.text', description)

                let id = cy.location('search')
                cy.log(cy.location('search')['Yielded'])


            })
        })

        it.skip('Edit news test', () => {

        })

        it.skip('Pin/unpin news test', () => {

        })
    })
})