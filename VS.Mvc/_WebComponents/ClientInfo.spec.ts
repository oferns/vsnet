import ClientInfo from "./ClientInfo";

describe("ClientInfo", () => {

    it("Should attach to the shadow root in the constructor", () => {
        // Arrange            
        // Act
        const element = new ClientInfo();

        // Assert
        expect(element.shadowRoot).toBeDefined();
    })
})