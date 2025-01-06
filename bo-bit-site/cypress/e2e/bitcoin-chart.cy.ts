describe('Bitcoin Price Chart E2E Test', () => {
    beforeEach(() => {
      // Visit the application
      cy.visit('http://localhost:4200/');
    });
  
    it('should display the chart with data', () => {
      cy.contains("Crypto: Bitcoin"); 
  
      cy.window().then((win) => {
        const performanceEntries = win.performance.getEntriesByType('resource');
        const apiCall = performanceEntries.find((entry: any) =>
          entry.name.includes('https://localhost:7214/BitcoinPrices')
        );
  
        expect(apiCall).to.exist;
      });
    });
  
    it('should dynamically update the chart with new data', () => {
      // Intercept the initial API call
      cy.intercept('GET', 'https://localhost:7214/BitcoinPrices', {
        statusCode: 200,
        body: {
            "maxPrice": 100000,
            "avgPrice": 100000,
            "from": "2025-01-06T10:50:35.9943059+00:00",
            "to": "2025-01-06T11:50:35.9943077+00:00",
            "cryptoCurrency": "Bitcoin",
            "fiatCurrency": "USD",
            "labels": ["2025-01-06T11:50:17+00:00"],
            "data": [100000]
      }}).as('initialFetch');
  
      // Wait for the next chart update
      cy.wait(28000);
      cy.wait('@initialFetch');
      cy.contains("Max price: $100,000"); 
    });
  });
  