import requests

url = "https://aeona3.p.rapidapi.com/"

querystring = {"text":"<REQUIRED>","userId":"12312312312"}

headers = {
	"X-RapidAPI-Key": "7d0b8de700msh80c32160ddc6996p1d2c39jsnd68dfd7f0d27",
	"X-RapidAPI-Host": "aeona3.p.rapidapi.com"
}

response = requests.get(url, headers=headers, params=querystring)

print(response.json())