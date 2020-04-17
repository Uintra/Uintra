


Cypress.Commands.add('selectEmoji', () => {

    cy.fixture('pagesElements.json')
        .then((p) => {

            const pagesElements = p
            cy.get(pagesElements.sosialPopup[0].emojiIcon).then(($icon) => {

                let iconSrc = $icon.attr('src')
                $icon.click()

                return cy.wrap(iconSrc)
            })
        })
})


Cypress.Commands.add('submitSocial', () => {
    cy.fixture('pagesElements.json')
        .then((p) => {
            const pagesElements = p

            cy.server()
            cy.route('POST', 'https://**//*ubaseline/api/social/create').as('createSocial')

            cy.get(pagesElements.sosialPopup[0].submitButton).click()

            cy.wait('@createSocial').then((xhr) => {
                expect(xhr.status).to.eq(200)
                let socialOriginalUrl = xhr.response.body.originalUrl

                cy.location().should((loc) => {
                    expect(loc.href).eq(Cypress.config().baseUrl + '/')
                })

                return cy.wrap(socialOriginalUrl)
            })
        })
})


Cypress.Commands.add('uploadFile', (filePath) => {
    cy.fixture('pagesElements.json')
        .then((p) => {
            const pagesElements = p
            cy.server()
            cy.route('POST', 'https://**/umbraco/api/file/UploadSingle').as('UploadSingle')

            cy.fixture(filePath, 'binary')
                .then(Cypress.Blob.binaryStringToBlob)
                .then((fileContent) => {
                    cy.get(pagesElements.sosialPopup[0].attachInput).attachFile({ fileContent, filePath: filePath, encoding: 'utf-8' });
                    cy.wait('@UploadSingle').then((xhr) => {
                        expect(xhr.status).to.eq(200)
                    })
                })
        })
})

