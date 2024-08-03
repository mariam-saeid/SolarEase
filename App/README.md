I contributed significantly to my graduation project by implementing the backend using ASP.NET Web API and SQL Server database. I developed prediction models employing Support Vector Regression (SVR) and created a rule-based chatbot that utilizes NLTK techniques such as tokenization, stemming, and Jaccard similarity. I integrated Google Maps API to sort locations and our models leveraged the OpenWeatherMap API to retrieve real-time features. Additionally, I deployed the backend on Monster ASP hosting, while the prediction models and chatbot were deployed using FastAPI on HuggingFace hosting.

-------------------------------------------------------------------------------------------------------------

Adopting solar cells is crucial for addressing climate change and transitioning to clean energy. Solar cells can power residential and commercial buildings, reducing carbon emissions and promoting a cleaner environment. The rising costs of traditional electricity sources lead to higher electricity bills, while solar power provides an independent energy source, reducing or eliminating these bills. Government incentives and the ability to sell excess energy back to the grid further enhance the financial benefits. Investing in solar energy offers long-term savings and environmental advantages. 

Our app provides a comprehensive solution that considers all scenarios of solar installation for accurate results, including a robust calculator for customized recommendations, an assistant chatbot for user guidance, a directory of certified installers sorted by nearest, and an online marketplace for seamless buying and selling of solar products. Additionally, users who already have solar systems can track their solar system's productivity.

-------------------------------------------------------------------------------------------------------------
Functional Requirements
1) Calculations:
- Calculate System Size: Provides the user with the appropriate solar system size and roof space needed for this system, including solar panels, inverter, and battery capacity, based on user-provided monthly electricity consumption and location.
- Calculate Cost: Provides the user with the total cost of the recommended solar system size. The cost calculation must consider the expenses associated with solar panels, inverters, batteries, installation, and any additional components.
- Calculate Financial Savings: Provides the user with the savings resulting from the recommended solar system size, considering the user's monthly, yearly, and 25-year savings. It also provides the user with a payback period for the solar system.
- Calculate Environmental Savings: Provides the user with the annual reduction in CO2 emissions that would be saved by installing a solar system, based on solar system size.

2) Prediction:
- Predict solar energy: provide the user with values every 3 hours and daily, spanning a five-day predictions of panel output. This prediction should be based on the user's solar system size and user’s location, as well as forecasted solar irradiance data.

3) Installation Guide:
- Find Solar Installers: Provides the user with a list of certified solar installation companies, ordered by the nearest to the user's location.
- Solar Market Prices: Provides the user with a price list of solar products including panels, inverters, and batteries from various brands and capacities. Users can add any products to their favorite products page for easy reference and comparison.
- Online Trade Marketplace: Users can post solar products for sale, whether new or used. These products must be displayed to other users interested in purchasing them. The marketplace should allow sellers to provide their contact information, such as a phone number, within their product posts to allow potential buyers to communicate with them. Users should be able to search, view, and add any post to their favorite posts page for easy reference and comparison.

4) Chatbot:
- Chatbot: The user can chat and interact with a rule-based chatbot that provides information on general solar-related topics, offers advice, and assists in facilitating the installation process.

5) Posts Filtration: 
- Filter Post: Admin can review and filter posts before publishing in the marketplace and reject the posts that violate the standards.
