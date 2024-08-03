from fastapi import FastAPI
from datetime import datetime
from itertools import groupby
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

            # Load the model
            model = joblib.load(model_path)
            
            # Load the scalers and cyclical transformer
            scaler = joblib.load(scaler_path)
            cyclical = joblib.load(cyclical_path)

            # Extract hourly forecast data for the next 5 days
            forecast_data = data["list"]

            # Initialize lists to store hourly and daily weather data
            hourly_list = []
            daily_list = []

            # Iterate through the hourly forecast data
            for hour in forecast_data:
                # Extract date and time for the forecast
                date_time = datetime.strptime(hour["dt_txt"], '%Y-%m-%d %H:%M:%S')

                # Add hourly data to the hourly list
                hourly_list.append({
                    "date": date_time,
                    "rain": hour["rain"]["3h"] if "rain" in hour else 0,
                    "temp": hour["main"]["temp"] if "main" in hour else 0,
                    "humidity": hour["main"]["humidity"] if "main" in hour else 0,
                    "pressure": hour["main"]["pressure"] / 10 if "main" in hour else 0,
                    "wind_speed": hour["wind"]["speed"] if "wind" in hour else 0
                })

            # Aggregate hourly data into daily averages or summaries
            grouped_by_date = groupby(hourly_list, lambda x: x["date"].date())
            for date, group in grouped_by_date:
                # Calculate daily averages or summaries
                group_list = list(group)
                daily_average_temp = sum(item["temp"] for item in group_list) / len(group_list)
                daily_max_rain = max(item["rain"] for item in group_list)
                daily_max_wind_speed = max(item["wind_speed"] for item in group_list)
                daily_average_humidity = sum(item["humidity"] for item in group_list) / len(group_list)
                daily_average_pressure = sum(item["pressure"] for item in group_list) / len(group_list)

                # Add daily data to the daily list
                daily_list.append({
                    "date": date.strftime('%Y-%m-%d'),
                    "average_temp": daily_average_temp,
                    "average_humidity": daily_average_humidity,
                    "max_rain": daily_max_rain,
                    "average_pressure": daily_average_pressure,
                    "max_wind_speed": daily_max_wind_speed,
                    "PredictedAllSky": 0
                })

                # Split date to year, month, day
                year = date.year
                month = date.month
                day = date.day
                last_item = daily_list[-1]

                # New data
                new_data = pd.DataFrame([[year, month, day, last_item["average_temp"], last_item["average_humidity"],
                                          last_item["max_rain"], last_item["average_pressure"], last_item["max_wind_speed"]]],
                                        columns=["YEAR", "MO", "DY", "T2M", "RH2M", "PRECTOTCORR", "PS", "WS10M"])

                # Apply cyclical transformation to the first 3 attributes
                new_data_cyclical = cyclical.transform(new_data[['YEAR', 'MO', 'DY']])

                # Apply scaling to the rest of the attributes
                new_data_scaled = scaler.transform(new_data[["T2M", "RH2M", "PRECTOTCORR", "PS", "WS10M"]])

                # Concatenate the transformed data
                new_data_transformed = np.concatenate((new_data_cyclical, new_data_scaled), axis=1)

                # Predict
                prediction = model.predict(new_data_transformed)
                last_item["PredictedAllSky"] = float(prediction[0])

            return daily_list

        return []

# Create a FastAPI app
app = FastAPI()

# Initialize the WeatherPredictionAPI with your API key
weather_api = WeatherPredictionAPI(api_key="")


@app.get('/api/Alexandria_daily')
def alexandria_daily_weather_forecast():
    latitude = 31.205753
    longitude = 29.924526
    model_path = r"alexandriaDaily_model_svr.joblib"
    scaler_path = r'alexandriaDaily_scaler_svr.pkl'
    cyclical_path = r'alexandriaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data


@app.get('/api/Asyut_daily')
def assiut_daily_weather_forecast():
    latitude = 27.180134
    longitude = 31.189283
    model_path = r"assiutDaily_model_svr.joblib"
    scaler_path = r'assiutDaily_scaler_svr.pkl'
    cyclical_path = r'assiutDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Aswan_daily')
def aswan_daily_weather_forecast():
    latitude = 24.0889
    longitude = 32.8998
    model_path = r"aswanDaily_model_svr.joblib"
    scaler_path = r'aswanDaily_scaler_svr.pkl'
    cyclical_path = r'aswanDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Beheira_daily')
def beheira_daily_weather_forecast():
    latitude = 30.8481
    longitude = 30.3436
    model_path = r"beheiraDaily_model_svr.joblib"
    scaler_path = r'beheiraDaily_scaler_svr.pkl'
    cyclical_path = r'beheiraDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Beni Suef_daily')
def beni_suef_daily_weather_forecast():
    latitude = 29.0661
    longitude = 31.0994
    model_path = r"beniSuefDaily_model_svr.joblib"
    scaler_path = r'beniSuefDaily_scaler_svr.pkl'
    cyclical_path = r'beniSuefDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Cairo_daily')
def cairo_daily_weather_forecast():
    latitude = 30.0444
    longitude = 31.2357
    model_path = r"cairoDaily_model_svr.joblib"
    scaler_path = r'cairoDaily_scaler_svr.pkl'
    cyclical_path = r'cairoDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Dakahlia_daily')
def dakahlia_daily_weather_forecast():
    latitude = 31.1656
    longitude = 31.4913
    model_path = r"dakahliaDaily_model_svr.joblib"
    scaler_path = r'dakahliaDaily_scaler_svr.pkl'
    cyclical_path = r'dakahliaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Damietta_daily')
def damietta_daily_weather_forecast():
    latitude = 31.4175
    longitude = 31.8144
    model_path = r"damiettaDaily_model_svr.joblib"
    scaler_path = r'damiettaDaily_scaler_svr.pkl'
    cyclical_path = r'damiettaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Faiyum_daily')
def fayoum_daily_weather_forecast():
    latitude = 29.3084
    longitude = 30.8428
    model_path = r"fayoumDaily_model_svr.joblib"
    scaler_path = r'fayoumDaily_scaler_svr.pkl'
    cyclical_path = r'fayoumDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Gharbia_daily')
def gharbia_daily_weather_forecast():
    latitude = 30.8754
    longitude = 31.0335
    model_path = r"gharbiaDaily_model_svr.joblib"
    scaler_path = r'gharbiaDaily_scaler_svr.pkl'
    cyclical_path = r'gharbiaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Giza_daily')
def giza_daily_weather_forecast():
    latitude = 30.0131
    longitude = 31.2089
    model_path = r"gizaDaily_model_svr.joblib"
    scaler_path = r'gizaDaily_scaler_svr.pkl'
    cyclical_path = r'gizaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Ismailia_daily')
def ismailia_daily_weather_forecast():
    latitude = 30.5965
    longitude = 32.2715
    model_path = r"ismailiaDaily_model_svr.joblib"
    scaler_path = r'ismailiaDaily_scaler_svr.pkl'
    cyclical_path = r'ismailiaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Kafr el-Sheikh_daily')
def kafr_el_sheikh_daily_weather_forecast():
    latitude = 31.1107
    longitude = 30.9388
    model_path = r"kafrEl-SheikhDaily_model_svr.joblib"
    scaler_path = r'kafrEl-SheikhDaily_scaler_svr.pkl'
    cyclical_path = r'kafrEl-SheikhDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Luxor_daily')
def luxor_daily_weather_forecast():
    latitude = 25.6872
    longitude = 32.6396
    model_path = r"luxorDaily_model_svr.joblib"
    scaler_path = r'luxorDaily_scaler_svr.pkl'
    cyclical_path = r'luxorDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Marsa Matruh_daily')
def matrouh_daily_weather_forecast():
    latitude = 31.3543
    longitude = 27.2373
    model_path = r"matrouhDaily_model_svr.joblib"
    scaler_path = r'matrouhDaily_scaler_svr.pkl'
    cyclical_path = r'matrouhDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Monufia_daily')
def menofia_daily_weather_forecast():
    latitude = 30.5972
    longitude = 30.9876
    model_path = r"menofiaDaily_model_svr.joblib"
    scaler_path = r'menofiaDaily_scaler_svr.pkl'
    cyclical_path = r'menofiaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Minya_daily')
def minya_daily_weather_forecast():
    latitude = 28.0871
    longitude = 30.7618
    model_path = r"minyaDaily_model_svr.joblib"
    scaler_path = r'minyaDaily_scaler_svr.pkl'
    cyclical_path = r'minyaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data


@app.get('/api/New Valley_daily')
def new_valley_daily_weather_forecast():
    latitude = 24.5456
    longitude = 27.1735
    model_path = r"newValleyDaily_model_svr.joblib"
    scaler_path = r'newValleyDaily_scaler_svr.pkl'
    cyclical_path = r'newValleyDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/North Sinai_daily')
def north_sinai_daily_weather_forecast():
    latitude = 30.2824
    longitude = 33.6176
    model_path = r"northSinaiDaily_model_svr.joblib"
    scaler_path = r'northSinaiDaily_scaler_svr.pkl'
    cyclical_path = r'northSinaiDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Port Said_daily')
def port_said_daily_weather_forecast():
    latitude = 31.2653
    longitude = 32.3019
    model_path = r"portSaidDaily_model_svr.joblib"
    scaler_path = r'portSaidDaily_scaler_svr.pkl'
    cyclical_path = r'portSaidDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Qena_daily')
def qena_daily_weather_forecast():
    latitude = 26.1551
    longitude = 32.7160
    model_path = r"qenaDaily_model_svr.joblib"
    scaler_path = r'qenaDaily_scaler_svr.pkl'
    cyclical_path = r'qenaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Qalyubia_daily')
def qualyubia_daily_weather_forecast():
    latitude = 30.2220
    longitude = 31.3084
    model_path = r"qualyubiaDaily_model_svr.joblib"
    scaler_path = r'qualyubiaDaily_scaler_svr.pkl'
    cyclical_path = r'qualyubiaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Red Sea_daily')
def red_sea_daily_weather_forecast():
    latitude = 24.6826
    longitude = 34.1532
    model_path = r"redSeaDaily_model_svr.joblib"
    scaler_path = r'redSeaDaily_scaler_svr.pkl'
    cyclical_path = r'redSeaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Sharqia_daily')
def al_sharqia_daily_weather_forecast():
    latitude = 30.7327
    longitude = 31.7195
    model_path = r"al-SharqiaDaily_model_svr.joblib"
    scaler_path = r'al-SharqiaDaily_scaler_svr.pkl'
    cyclical_path = r'al-SharqiaDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Sohag_daily')
def sohag_daily_weather_forecast():
    latitude = 26.5591
    longitude = 31.6957
    model_path = r"sohagDaily_model_svr.joblib"
    scaler_path = r'sohagDaily_scaler_svr.pkl'
    cyclical_path = r'sohagDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/South Sinai_daily')
def south_sinai_daily_weather_forecast():
    latitude = 28.9710
    longitude = 33.6176
    model_path = r"southSinaiDaily_model_svr.joblib"
    scaler_path = r'southSinaiDaily_scaler_svr.pkl'
    cyclical_path = r'southSinaiDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data

@app.get('/api/Suez_daily')
def suez_daily_weather_forecast():
    latitude = 29.9668
    longitude = 32.5498
    model_path = r"suezDaily_model_svr.joblib"
    scaler_path = r'suezDaily_scaler_svr.pkl'
    cyclical_path = r'suezDaily_cyclical_svr.pkl'
    data = weather_api.get_weather_forecast(latitude, longitude, model_path, scaler_path, cyclical_path)
    return data
