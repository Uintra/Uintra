/** create TODO:
 * with text + 
 * with image+
 * with video +
 * with smile only +
 * all together data
 * other files attachment +
 * video convertation to mp4 / move to separated test
 */

/** TODO ref:
 * move file attach to comands
 */

let pagesElements
let userData;

context("Social activivty create test", () => {
    beforeEach(() => {
        // Login
        cy.fixture('userData.json')
            .then((uD) => {
                const user = uD
                cy.login({ email: user.email, password: user.password })
                userData = uD
            })

        //Other fixtures 
        cy.fixture('pagesElements.json')
            .then((np) => pagesElements = np)
    })

    it("Create social activity only with text", () => {
        /**TODO: change to lorem */
        cy.lorem().then(lorem => {
            cy.openSocialCreatePopup()
            let text = lorem.generateWords(2)
            cy.get(pagesElements.sosialPopup[0].textInput).type(text)

            cy.submitSocial().then(actUrl => {

                //Check sosial details page 
                cy.visit(actUrl)
                cy.get(pagesElements.activityDetailsPage[0].ownerName).should('have.text', userData.firstName + ' ' + userData.lastName)
                cy.get(pagesElements.activityDetailsPage[0].activityName).should('have.text', 'Socials')
                cy.get(pagesElements.activityDetailsPage[0].description).should('have.text', text)

            })
        })
    })

    it('Create social activity only with emoji', () => {
        cy.openSocialCreatePopup()

        cy.get(pagesElements.sosialPopup[0].emojiSelector).click()
        cy.get(pagesElements.sosialPopup[0].emojiList).should('be.visible')

        cy.selectEmoji().then(src => {
            let iconSrc = src

            cy.submitSocial().then(actUrl => {
                let socialOriginalUrl = actUrl
                cy.visit(socialOriginalUrl)

                //Check sosial details page 
                cy.get(pagesElements.activityDetailsPage[0].ownerName).should('have.text', userData.firstName + ' ' + userData.lastName)
                cy.get(pagesElements.activityDetailsPage[0].activityName).should('have.text', 'Socials')
                cy.get(pagesElements.activityDetailsPage[0].description).children().then(($icon) => {
                    expect($icon.attr('src')).to.include(iconSrc)
                })
            })
        })
    })

    it('Create social activity only with image', () => {
        const filePath = 'image.png'

        cy.openSocialCreatePopup()

        cy.uploadFile(filePath).then((xhr) => {

                cy.submitSocial().then(actUrl => {
                    let socialOriginalUrl = actUrl

                    cy.visit(socialOriginalUrl)

                    cy.get(pagesElements.activityDetailsPage[0].ownerName).should('have.text', userData.firstName + ' ' + userData.lastName)
                    cy.get(pagesElements.activityDetailsPage[0].activityName).should('have.text', 'Socials')
                    cy.get(pagesElements.activityDetailsPage[0].fileAttach).then(($file) => {
                        expect($file.attr('src')).to.include(filePath)
                    })

                })
        })
    })


    it("Create social activity only with .pdf", () => {

        const filePath = 'test.pdf'

        cy.openSocialCreatePopup()

        cy.uploadFile(filePath).then((xhr) => {

                cy.submitSocial().then(actUrl => {
                    let socialOriginalUrl = actUrl

                    cy.visit(socialOriginalUrl)

                    cy.get(pagesElements.activityDetailsPage[0].ownerName).should('have.text', userData.firstName + ' ' + userData.lastName)
                    cy.get(pagesElements.activityDetailsPage[0].activityName).should('have.text', 'Socials')
                    cy.get(pagesElements.activityDetailsPage[0].docIcon).should('have.text', 'pdf')
                    cy.get(pagesElements.activityDetailsPage[0].docTitle).should(($docTitle) => {
                        expect($docTitle.first()).to.contain(filePath)
                    })
                })
        })
    })


    it("Create social activity only with video", () => {
        const filePath = 'sample.mp4'
        const videoName = 'sample'

        cy.openSocialCreatePopup()

        cy.uploadFile(filePath).then((xhr) => {

            cy.submitSocial().then(actUrl => {
                let socialOriginalUrl = actUrl

                cy.visit(socialOriginalUrl)

                cy.get(pagesElements.activityDetailsPage[0].ownerName).should('have.text', userData.firstName + ' ' + userData.lastName)
                cy.get(pagesElements.activityDetailsPage[0].activityName).should('have.text', 'Socials')
                cy.get(pagesElements.activityDetailsPage[0].fileAttach).then(($file) => {
                    expect($file.attr('src')).to.include(videoName + '.jpg?preset=preview')
                })
            })
        })
    })


    it("Create social activity with text, emoji, image, .pdf, video", () => {

        cy.lorem().then(lorem => {
            let text = lorem.generateWords(2)
            const imagePath = 'image.png'
            const pdfPath = 'test.pdf'
            const videoPath = 'sample.mp4'
            const videoName = 'sample' + '.jpg?preset=preview'

            cy.openSocialCreatePopup()

            cy.get(pagesElements.sosialPopup[0].textInput).type(text)
            cy.get(pagesElements.sosialPopup[0].emojiSelector).click()
            cy.get(pagesElements.sosialPopup[0].emojiList).should('be.visible')

            cy.selectEmoji().then(src => {
                let iconSrc = src

                cy.uploadFile(imagePath).then((xhr) => {
                    cy.uploadFile(pdfPath).then((xhr) => {
                        cy.uploadFile(videoPath).then((xhr) => {

                            cy.submitSocial().then(actUrl => {

                                cy.visit(actUrl)

                                cy.get(pagesElements.activityDetailsPage[0].ownerName).should('have.text', userData.firstName + ' ' + userData.lastName)
                                cy.get(pagesElements.activityDetailsPage[0].activityName).should('have.text', 'Socials')
                                cy.get(pagesElements.activityDetailsPage[0].description).should('have.text', text)

                                cy.get(pagesElements.activityDetailsPage[0].docIcon).should('have.text', 'pdf')
                                cy.get(pagesElements.activityDetailsPage[0].docTitle).should(($docTitle) => {
                                    expect($docTitle.first()).to.contain(pdfPath)
                                })

                                cy.get(pagesElements.activityDetailsPage[0].fileAttach).then(($file) => {
                                    expect($file.attr('src')).to.include(imagePath + '?preset=previewTwo')
                                })

                                cy.get('[src*="' + videoName + '"]').should(($p) => {
                                    expect($p).to.have.length(1)
                                })
                            })


                        })

                    })

                })
            })
        })
    })
})