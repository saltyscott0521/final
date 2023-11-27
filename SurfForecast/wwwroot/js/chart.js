
//first fetch api
/*const key = "51de08d7fc9bfbc15136270d5738855f";
const header = {
  'Content-Type': 'application/json'
};
const url = `https://api.openweathermap.org/data/2.5/forecast?lat=44.34&lon=10.99&appid=${key}`;
console.log(url);

fetch(url)
  .then(response => {
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    return response.json(); // Parse the response as JSON
  })
  .then(data => {   
    const datetime = data.list.map(item => item.dt_txt);
    const windSpeed = data.list.map(item => item.wind.speed);

new Chart(document.getElementById("windspeed-chart"), {
  type: 'line',
  data: {
    labels: datetime,
    datasets: [
      {
        label: "Wind Speed (mph)",
        fill: true,
        borderColor: "#4169e1",
        data: windSpeed
      }
    ]
  },
  options: {
    legend: { display: false },
    title: {
      display: true,
      text: 'Wind Speed (mph)'
    }
  }
});

new Chart(document.getElementById("waveheight-chart"), {
  type: 'line',
  data: {
    labels: datetime,
    datasets: [
      {
        label: "Wave Height (ft)",
        fill: true,
        borderColor: "#4169e1",
        data: windSpeed
      }
    ]
  },
  options: {
    legend: { display: false },
    title: {
      display: true,
      text: 'Wave Height (ft)'
    }
  }
});


  })
  .catch(error => {
    console.error('There was a problem with the fetch operation:', error);
  });

// Create chart
*/