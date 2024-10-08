from fastapi import FastAPI

# Importing NLTK and related libraries for text processing
import nltk
from nltk.tokenize import word_tokenize
from nltk.stem import PorterStemmer
nltk.download('punkt')

# Sample data (organized into segments)
data = {
    "Hello":"Hello! How can I assist you today?",
    
    "Hi":"Hi there! What can I do for you today?",

    "Hey":"Hey there! What can I do for you today?",
    
    "What is your name?":"I am SolarEase Bot, your assistant for solar energy inquiries.",
    
    "How can you help me?":"I can provide information about solar energy and answer common questions you might have.",
    
    "What do you do?":"I assist with questions related to solar energy, our products, and general information about solar power systems.",
    
    "Thank":"You are welcome! If you have any more questions, feel free to ask.",
    
    "Who created you?": "I was created by the SolarEase team to help you with your solar energy questions.",

    "bye": "Goodbye!",
    
    "quit": "Goodbye!",
    
    "exit": "Goodbye!",
    
    "Can you tell me a joke?":"Sure! Why did the solar panel bring a book to the meeting? Because it wanted to have some light reading!",

    "What is SolarEase?": "SolarEase is an application that helps you install solar systems and track predictions for your system's energy production.",
    
    "What are the features and services of SolarEase?": "SolarEase offers various features, including System Size Calculation, Marketplace, Chatbot, Solar Installer Finder, and Energy Production Prediction.",
    
    "What is the Calculating feature?": "Calculating feature helps you determine the appropriate system size and its components based on your needs. It also provides information on financial savings, payback period, and environmental benefits.",
    
    "What is the Marketplace?": "The Marketplace allows users to buy and sell solar components through posts. You can also see average prices of solar components (panels, inverters, and batteries) in the real market.",
    
    "What is the Chatbot?": "The Chatbot provides answers to a variety of questions about solar energy, helping you learn more about the topic.",
    
    "What is the Find Solar Installer feature?": "Solar Installer feature provides information about solar companies from nearest to farthest based on your location.",
    
    "What is the Prediction feature?": "The Prediction feature forecasts the amount of electricity your system will produce by the hour and for the next five days based on your location and system size.",

    "system sizing": "System Size Calculated by analyzing past energy bills, roof space, and local sunlight conditions using assessment tools or professional evaluations.",
    
    "on grid vs off grid difference": "On Grid vs Off Grid;Off-Grid Systems: Not connected to the public electricity grid, relies on batteries to store energy for use when solar power is unavailable.;On-Grid Systems: Connected to the public electricity grid, can export excess electricity back to the grid. Grid type determined Based on Location",

    "Which solar system is more appropriate": "Solar System is more Appropriate;Off-Grid Systems: Suitable for isolated places where access to regular electricity is not available.;On-Grid Systems: Ideal for residential areas with regular electricity and for households using over 1000 kWh per month.",
    
    "pros cons on grid advantage disadvantage": "On Grid;Pros:;Typically lower installation costs.;Reduces electricity bills through net metering.;Credits for excess energy fed back to the grid.;No need for batteries.;Reliable power supply from the grid when solar production is insufficient.;Easier to set up with existing infrastructure.;Cons:;No power during grid outages without backup solutions.;Subject to permits and local policies.;Cannot use excess energy during non-productive times without storage.;Changes in net metering policies can reduce financial returns by decreasing credits for excess solar energy fed back to the grid.",
    
    "pros cons off grid advantage disadvantage": "Off Grid;Pros:;Not reliant on the public grid.;Eliminates monthly electricity costs.;Ideal for areas without grid access.;Fully reliant on renewable energy.;Can be expanded with more panels and batteries.;Greater independence in managing energy.;Cons:;More expensive due to battery requirements.;Requires investment in and maintenance of batteries.;Limited by battery capacity during non-productive times.;Affected by weather and seasonal changes.;May require additional power sources during low sunlight periods.",
    
    "exchange meter": "Exchange Meter is also called as Net meter or power meter, it is In the case of an on-grid solar system only to measures both the amount of electricity consumed by a household and the amount of electricity generated by the solar system and fed back into the grid. This allows for accurate monitoring of energy usage and generation.",
    
    "meter installation": "Meter Installation;If the existing meter is one phase it must be changed to a 3-phase meter before installing the new exchange meter for the solar system.;Then install the exchange meter before proceeding with the installation of the solar system.;Finally, proceed with the installation of the solar system after the exchange meter has been installed.",
    
    "financial support": "Financial Support;Banks offer loans for residential installations, while governments provide financial incentives and grants for commercial installations.",
    
    "pros cons solar energy": "Solar Energy;Pros:;Renewable and Sustainable.;Reduces Electricity Bills.;Low Operating Costs.;Environmentally Friendly.;Energy Independence.;Versatile Applications.;Job Creation.;Technological Advancements.;Increases Property Value.;Peak Power Generation.;Cons:;High Initial Costs.;Weather Dependent.;Energy Storage Costs.;Space Requirements.;Intermittent Energy Production.;Efficiency Limitations.;Environmental Impact of Production.;Geographic Limitations.;Policy and Regulatory Risks.;Grid Integration Challenges.",
    
    "environmental benefits": "Environmental Benefits;Solar energy is a clean, renewable source that reduces reliance on fossil fuels. When a solar system generates electricity, it produces no greenhouse gas emissions.;Each kilowatt-hour (kWh) of solar energy results in an estimated reduction of 0.45-0.5 kg of CO2 emissions compared to conventional fossil fuel energy sources. (4 kWh save 2 kg of CO2 daily).",
    
    "financial benefits": "Financial Benefits;In case of on-grid households using over 1000 kWh per month, Solar energy reduces electricity bills, increases property value, and offers potential income through net metering credits for excess energy.;Additionally, it provides protection against rising energy costs and can qualify for various tax incentives and rebates.",
    
    "solar power system": "Solar power system converts sunlight into electricity using solar panels. Typically includes solar panels, an inverter, mounting equipment, and sometimes a battery for energy storage.",
    
    "solar panels work": "Solar panels use photovoltaic (PV) cells to convert sunlight into direct current (DC) electricity. Sunlight hitting the cells creates a flow of electricity by knocking electrons loose.",
    
    "solar inverter": "Solar inverter converts DC electricity from solar panels into alternating current (AC) electricity for home appliances and grid compatibility.",
    
    "solar battery": "Solar battery stores excess electricity generated by solar panels for use when sunlight is not available, ensuring a continuous power supply.",
    
    "solar charge controller": "Solar charge controller regulates voltage and current from the solar panels to the battery, preventing overcharging and prolonging battery life.",
    
    "solar array": "Solar Array is a group of multiple solar panels connected together to form a complete solar power system, increasing the total power generation capacity.",
    
    "roof ownership renting": "Roof Ownership Renting;You can install solar panels if you either own the entire roof or have a long-term lease (25 years) sufficient for the system's lifespan.;For partial ownership, securing an agreement from other co-owners or stakeholders is necessary. Approval from co-owners ensures legal compliance, avoids disputes, and addresses any shared responsibilities.",
    
    "space requirements": "Space Requirements;The space needed for a solar panel system depends on the efficiency of the panels and the amount of available sunlight.;Typically, 1 kW of solar panels requires 8 to 10 square meters of area.",
    
    "switching on grid regular electricity": "Switching on grid Regular Electricity;You will need to sell the system and cancel the contract with the public electricity grid company.;The installed exchange meter will remain but only count the electricity consumed from the grid, rather than measuring bidirectional energy flow.",
    
    "selling apartment with solar system": "Selling Apartment with Aolar System;If the solar system is Included in the Sale:;The buyer must transfer ownership of the system to themselves.;If the solar system is not Included in Sale:;Do the following:;1-Cancel the current contract.;2-Move the solar system to your new residence.;3-Set up a new contract for the solar installation at the new location.",
    
    "excess deficit electricity": "Excess Deficit Electricity;In an on-grid system, sometimes the solar panels generate more electricity than the user needs. In such cases, the excess electricity is exported to the public electricity grid. This scenario occurs daily throughout the year, until June 30th, the public grid company compensates the user for the excess electricity at a rate that is typically lower than what the user pays for electricity consumed from the grid.;In an on-grid system, sometimes the user requires more electricity than their solar panels generate, they will obtain the additional electricity from the public electricity grid company. At the end of the month, the user will be billed for the electricity consumed from the grid.",
    
    "factors affecting longevity": "Factors Affecting Longevity;Quality of components, Installation quality, Environmental conditions (e.g., weather, temperature), Maintenance practices, Usage patterns.",
    
    "typical lifespan": "Typical Lifespan;Solar Panels: 25-30 years; Inverters: 10-15 years; Batteries: 5-15 years, depending on the type (lead-acid, lithium-ion, etc.).",
    
    "warranty coverage": "Warranty coverage reduces long-term maintenance costs by covering repair and replacement of defective components. Extended warranties and service contracts can minimize unexpected expenses and ensure consistent system performance. Helps in budgeting and financial planning for the solar power system's upkeep.",
    
    "regular maintenance tasks": "Regular Maintenance Tasks;Clean panels to remove dirt, dust, and debris that can reduce efficiency.;Check for any physical damage or shading from nearby objects or vegetation.;Monitor electrical connections for signs of wear or corrosion.",
    
    "frequency inspections cleaning": "Frequency Inspections Cleaning;Inspect panels at least twice a year, ideally before and after the harshest seasons (e.g., winter and summer).;Clean panels as needed, typically every 6 to 12 months depending on local weather conditions and dust levels.",

    "What is the estimated cost of solar system installation?" : "For on-grid systems, the estimated cost is 30,000 EGP per kilowatt (kW). Please note that these are rough estimates. For a precise calculation, we recommend using our solar calculator."
}


# Initialize FastAPI app
app = FastAPI()

# Initialize NLTK's WordNet Lemmatizer
stemmer = PorterStemmer()

# Function to calculate similarity (using tokenization, stemming, and set operations)
def calculate_similarity(query, text):
    # Tokenize and stem the query and text
    query_tokens = set([stemmer.stem(token) for token in word_tokenize(query.lower())])
    text_tokens = set([stemmer.stem(token) for token in word_tokenize(text.lower())])
    
    # Calculate Jaccard similarity
    intersection = query_tokens.intersection(text_tokens)
    union = query_tokens.union(text_tokens)
    
    # Adjusted Jaccard similarity score based on the length of intersection and union
    jaccard_score = len(intersection) / len(union) if len(union) > 0 else 0
    
    return jaccard_score

# Function to find the best match for user query
def find_best_match(query):
    best_key = None
    best_value = None
    max_score = 0
    
    for key, value in data.items():
        score = calculate_similarity(query, key)
        
        # Implement substring matching
        if query.lower() in key.lower():
            score += 0.5
        
        if score > max_score:
            max_score = score
            best_key = key
            best_value = value
    
    return best_key, best_value

# Route to handle queries
@app.get("/query/")
def query_endpoint(query: str):
    best_key, best_value = find_best_match(query)
    
    if best_key:
        return {"query": query, "best_match": best_key, "best_match_text": best_value}
    else:
        return {"query": query, "best_match": "Not found", "best_match_text": "No relevant information found for the query."}
