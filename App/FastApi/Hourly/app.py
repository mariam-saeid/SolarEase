from fastapi import FastAPI
from datetime import datetime
import numpy as np
import pandas as pd
import joblib
import requests

class WeatherPredictionAPI:
    def __init__(self, api_key: str):
        self.api_key = api_key

    def get_weather_forecast(self, latitude: float, longitude: float, model_path: str, scaler_path: str, cyclical_path: str):
        # API endpoint for hourly weather forecast data
        url = f"http://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&appid={self.api_key}&units=metric"

        # Make a GET request to the API
        response = requests.get(url)

        # Check if the request was successful (status code 200)
        if response.status_code == 200:
            # Convert the response to JSON format
            data = response.json()

            # Extract relevant information from the response
            forecast_data = data["list"]

            # Load the model
            model = joblib.load(model_path)
            
            # Load the scalers and cyclical transformer
            scaler = joblib.load(scaler_path)
            cyclical = joblib.load(cyclical_path)

            hourly_list = []

            # Print hourly forecast data for the next 5 days
            for hour in forecast_data:
                # Extract date and time for the forecast
                date_time = datetime.strptime(hour["dt_txt"], '%Y-%m-%d %H:%M:%S')

                # Add hourly data to the list
                hourly_list.append({
                    "date": date_time.strftime('%Y-%m-%d %H:%M:%S'),
                    "temp": hour["main"]["temp"] if "main" in hour else 0,
                    "humidity": hour["main"]["humidity"] if "main" in hour else 0,
                    "rain": hour["rain"]["3h"] if "rain" in hour else 0,
                    "pressure": hour["main"]["pressure"] / 10 if "main" in hour else 0,
                    "wind_speed": hour["wind"]["speed"] if "wind" in hour else 0,
                    "PredictedAllSky": 0
                })

                # Split date and time into year, month, day, and hour
                year = date_time.year
                month = date_time.month
                day = date_time.day
                hourofDay = date_time.hour
                last_item = hourly_list[-1]

                if 5 <= hourofDay <= 20:
                    # New data
                    new_data = pd.DataFrame([[year, month, day, hourofDay, last_item["temp"], last_item["humidity"], last_item["rain"], last_item["pressure"], last_item["wind_speed"]]], columns=["YEAR", "MO", "DY", "HR","T2M", "RH2M", "PRECTOTCORR", "PS", "WS10M"])
                    
                    # Apply cyclical transformation to the first 4 attributes
                    new_data_cyclical = cyclical.transform(new_data[['YEAR', 'MO', 'DY', 'HR']])
                    
                    # Apply scaling to the rest of the attributes
                    new_data_scaled = scaler.transform(new_data[["T2M", "RH2M", "PRECTOTCORR", "PS", "WS10M"]])
                    
                    # Concatenate the transformed data
                    new_data_transformed = np.concatenate((new_data_cyclical, new_data_scaled), axis=1)
                    
                    # Predict
                    prediction = model.predict(new_data_transformed)
                    last_item["PredictedAllSky"] = float(prediction[0])
                    if last_item["PredictedAllSky"] < 0:
                        last_item["PredictedAllSky"] = 0
            
            return hourly_list

        return []

# Create a FastAPI app
app = FastAPI()

# Initialize the WeatherPredictionAPI with your API key
weather_api = WeatherPredictionAPI(api_key="")

@app.get('/api/Alexandria_hourly')
def alexandria_hourly_weather_forecast():
    latitude = 31.205753
    longitude = 29.924526
    model_path = r"alexandriaHourly_model_svr.joblib"
    scaler_path = r'alexandriaHourly_scaler_svr.pkl'
    cyclical_path = r'alexandriaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Asyut_hourly')
def assiut_hourly_weather_forecast():
    latitude = 27.180134
    longitude = 31.189283
    model_path = r"assiutHourly_model_svr.joblib"
    scaler_path = r'assiutHourly_scaler_svr.pkl'
    cyclical_path = r'assiutHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Aswan_hourly')
def aswan_hourly_weather_forecast():
    latitude = 24.0889
    longitude = 32.8998
    model_path = r"aswanHourly_model_svr.joblib"
    scaler_path = r'aswanHourly_scaler_svr.pkl'
    cyclical_path = r'aswanHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Beheira_hourly')
def beheira_hourly_weather_forecast():
    latitude = 30.8481
    longitude = 30.3436
    model_path = r"beheiraHourly_model_svr.joblib"
    scaler_path = r'beheiraHourly_scaler_svr.pkl'
    cyclical_path = r'beheiraHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Beni Suef_hourly')
def beni_suef_hourly_weather_forecast():
    latitude = 29.0661
    longitude = 31.0994
    model_path = r"beniSuefHourly_model_svr.joblib"
    scaler_path = r'beniSuefHourly_scaler_svr.pkl'
    cyclical_path = r'beniSuefHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Cairo_hourly')
def cairo_hourly_weather_forecast():
    latitude = 30.0444
    longitude = 31.2357
    model_path = r"cairoHourly_model_svr.joblib"
    scaler_path = r'cairoHourly_scaler_svr.pkl'
    cyclical_path = r'cairoHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Dakahlia_hourly')
def dakahlia_hourly_weather_forecast():
    latitude = 31.1656
    longitude = 31.4913
    model_path = r"dakahliaHourly_model_svr.joblib"
    scaler_path = r'dakahliaHourly_scaler_svr.pkl'
    cyclical_path = r'dakahliaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Damietta_hourly')
def damietta_hourly_weather_forecast():
    latitude = 31.4175
    longitude = 31.8144
    model_path = r"damiettaHourly_model_svr.joblib"
    scaler_path = r'damiettaHourly_scaler_svr.pkl'
    cyclical_path = r'damiettaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Faiyum_hourly')
def fayoum_hourly_weather_forecast():
    latitude = 29.3084
    longitude = 30.8428
    model_path = r"fayoumHourly_model_svr.joblib"
    scaler_path = r'fayoumHourly_scaler_svr.pkl'
    cyclical_path = r'fayoumHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Gharbia_hourly')
def gharbia_hourly_weather_forecast():
    latitude = 30.8754
    longitude = 31.0335
    model_path = r"gharbiaHourly_model_svr.joblib"
    scaler_path = r'gharbiaHourly_scaler_svr.pkl'
    cyclical_path = r'gharbiaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Giza_hourly')
def giza_hourly_weather_forecast():
    latitude = 30.0131
    longitude = 31.2089
    model_path = r"gizaHourly_model_svr.joblib"
    scaler_path = r'gizaHourly_scaler_svr.pkl'
    cyclical_path = r'gizaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Ismailia_hourly')
def ismailia_hourly_weather_forecast():
    latitude = 30.5965
    longitude = 32.2715
    model_path = r"ismailiaHourly_model_svr.joblib"
    scaler_path = r'ismailiaHourly_scaler_svr.pkl'
    cyclical_path = r'ismailiaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Kafr el-Sheikh_hourly')
def kafr_el_sheikh_hourly_weather_forecast():
    latitude = 31.1107
    longitude = 30.9388
    model_path = r"kafrEl-SheikhHourly_model_svr.joblib"
    scaler_path = r'kafrEl-SheikhHourly_scaler_svr.pkl'
    cyclical_path = r'kafrEl-SheikhHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Luxor_hourly')
def luxor_hourly_weather_forecast():
    latitude = 25.6872
    longitude = 32.6396
    model_path = r"luxorHourly_model_svr.joblib"
    scaler_path = r'luxorHourly_scaler_svr.pkl'
    cyclical_path = r'luxorHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Marsa Matruh_hourly')
def matrouh_hourly_weather_forecast():
    latitude = 31.3543
    longitude = 27.2373
    model_path = r"matrouhHourly_model_svr.joblib"
    scaler_path = r'matrouhHourly_scaler_svr.pkl'
    cyclical_path = r'matrouhHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Monufia_hourly')
def menofia_hourly_weather_forecast():
    latitude = 30.5972
    longitude = 30.9876
    model_path = r"menofiaHourly_model_svr.joblib"
    scaler_path = r'menofiaHourly_scaler_svr.pkl'
    cyclical_path = r'menofiaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Minya_hourly')
def minya_hourly_weather_forecast():
    latitude = 28.0871
    longitude = 30.7618
    model_path = r"minyaHourly_model_svr.joblib"
    scaler_path = r'minyaHourly_scaler_svr.pkl'
    cyclical_path = r'minyaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/New Valley_hourly')
def new_valley_hourly_weather_forecast():
    latitude = 24.5456
    longitude = 27.1735
    model_path = r"newValleyHourly_model_svr.joblib"
    scaler_path = r'newValleyHourly_scaler_svr.pkl'
    cyclical_path = r'newValleyHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/North Sinai_hourly')
def north_sinai_hourly_weather_forecast():
    latitude = 30.2824
    longitude = 33.6176
    model_path = r"northSinaiHourly_model_svr.joblib"
    scaler_path = r'northSinaiHourly_scaler_svr.pkl'
    cyclical_path = r'northSinaiHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Port Said_hourly')
def port_said_hourly_weather_forecast():
    latitude = 31.2653
    longitude = 32.3019
    model_path = r"portSaidHourly_model_svr.joblib"
    scaler_path = r'portSaidHourly_scaler_svr.pkl'
    cyclical_path = r'portSaidHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Qena_hourly')
def qena_hourly_weather_forecast():
    latitude = 26.1551
    longitude = 32.7160
    model_path = r"qenaHourly_model_svr.joblib"
    scaler_path = r'qenaHourly_scaler_svr.pkl'
    cyclical_path = r'qenaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Qalyubia_hourly')
def qualyubia_hourly_weather_forecast():
    latitude = 30.2220
    longitude = 31.3084
    model_path = r"qualyubiaHourly_model_svr.joblib"
    scaler_path = r'qualyubiaHourly_scaler_svr.pkl'
    cyclical_path = r'qualyubiaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Red Sea_hourly')
def red_sea_hourly_weather_forecast():
    latitude = 24.6826
    longitude = 34.1532
    model_path = r"redSeaHourly_model_svr.joblib"
    scaler_path = r'redSeaHourly_scaler_svr.pkl'
    cyclical_path = r'redSeaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Sharqia_hourly')
def al_sharqia_hourly_weather_forecast():
    latitude = 30.7327
    longitude = 31.7195
    model_path = r"al-SharqiaHourly_model_svr.joblib"
    scaler_path = r'al-SharqiaHourly_scaler_svr.pkl'
    cyclical_path = r'al-SharqiaHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Sohag_hourly')
def sohag_hourly_weather_forecast():
    latitude = 26.5591
    longitude = 31.6957
    model_path = r"sohagHourly_model_svr.joblib"
    scaler_path = r'sohagHourly_scaler_svr.pkl'
    cyclical_path = r'sohagHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/South Sinai_hourly')
def south_sinai_hourly_weather_forecast():
    latitude = 28.9710
    longitude = 33.6176
    model_path = r"southSinaiHourly_model_svr.joblib"
    scaler_path = r'southSinaiHourly_scaler_svr.pkl'
    cyclical_path = r'southSinaiHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Suez_hourly')
def suez_hourly_weather_forecast():
    latitude = 29.9668
    longitude = 32.5498
    model_path = r"suezHourly_model_svr.joblib"
    scaler_path = r'suezHourly_scaler_svr.pkl'
    cyclical_path = r'suezHourly_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data