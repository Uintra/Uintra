context('user login test', () => {

  let userData
  let pagesElements
  let validationMassages

  beforeEach(() => {
    cy.visit('/login').waitForBrowser

    cy.fixture('userData.json')
      .then((uD) => userData = uD)

    cy.fixture('pagesElements.json')
      .then((p) => pagesElements = p)

    cy.fixture('validationMassages.json')
      .then((v) => validationMassages = v)

  })

  it('user sucessfully login', () => {

    //Fill login form
    cy.xpath(pagesElements.loginPage[0].loginInput)
      .type(userData.email)

      .xpath(pagesElements.loginPage[0].passwordInput)
      .type(userData.password)

      .xpath(pagesElements.loginPage[0].loginButton)
      .click()
      .waitForBrowser;

    //Test asserts 
    //check header
    cy.get(pagesElements.header[0].userName).should('have.text', userData.firstName + ' ' + userData.lastName)

  })

  it('login form validation check', () => {

    cy.xpath(pagesElements.loginPage.validationMessage).should('have.length', 0)
    //Fill login form
    cy.xpath(pagesElements.loginPage[0].loginInput)
      .should('be.visible')
      .should('have.length', 1)
      .type(userData.email + "1")

      .xpath(pagesElements.loginPage[0].passwordInput)
      .should('be.visible')
      .should('have.length', 1)
      .type(userData.password + "1")

      .xpath(pagesElements.loginPage[0].loginButton)
      .should('be.visible')
      .should('have.length', 1)
      .click()
      .waitForBrowser;

    //Test asserts 
    //Check validation message
    cy.get(pagesElements.loginPage[0].validationMessage).should('contain.text',validationMassages.loginFormValidation)
  })
})