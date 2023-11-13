
// Observer interface from the weather station showing available weather data
public interface IObserver
{
    void Update(float temperature, float humidity, float pressure);
}

// Observable interface with method to add, remove and/or notify the different displays of new updates
public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}

// Weather data object
public class WeatherData : ISubject
{
    private List<IObserver> observers;
    private float temperature;
    private float humidity;
    private float pressure;

    public WeatherData()
    {
        observers = new List<IObserver>();
    }

    // Register a new observer
    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    // Remove an existing observer
    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    // Notify all observers of changes in weather conditions
    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(temperature, humidity, pressure);
        }
    }

    // Set new measurements and notify observers
    public void SetMeasurements(float temperature, float humidity, float pressure)
    {
        this.temperature = temperature;
        this.humidity = humidity;
        this.pressure = pressure;
        MeasurementChanged();
    }

    // This method will be called whenever measurements change to get the new weather Data
    private void MeasurementChanged()
    {
        NotifyObservers();
    }
}

// Concrete observer for the current weather information display
//The pressure value is received, but it is not displayed
public class CurrentConditionsDisplay : IObserver
{
    private float temperature;
    private float humidity;

    public void Update(float temperature, float humidity, float pressure)
    {
        this.temperature = temperature;
        this.humidity = humidity;
        Display();
    }

    public void Display()
    {
        Console.WriteLine($"Current conditions: {temperature}F degrees and {humidity}% humidity");
    }
}

// Concrete observer representing a statistics display
public class StatisticsDisplay : IObserver
{
    private float maxTemperature = float.MinValue;
    private float minTemperature = float.MaxValue;
    private float temperatureSum = 0;
    private int numReadings = 0;

    public void Update(float temperature, float humidity, float pressure)
    {
        temperatureSum += temperature;
        numReadings++;

        if (temperature > maxTemperature)
            maxTemperature = temperature;

        if (temperature < minTemperature)
            minTemperature = temperature;

        Display();
    }

    public void Display()
    {
        Console.WriteLine($"Avg/Max/Min temperature = {temperatureSum / numReadings}/{maxTemperature}/{minTemperature}");
    }
}

// Concrete observer representing a simple forecast display
// Concrete observer representing a forecast display with different strategies
public class ForecastDisplay : IObserver
{
    public void Update(float temperature, float humidity, float pressure)
    {
        Display(temperature, humidity, pressure);
    }

    public void Display(float temperature, float humidity, float pressure)
    {
        if (temperature == 80 && humidity == 65)
        {
            DisplayHumidAndHotForecast();
        }
        else if (temperature == 82 && humidity >= 70)
        {
            DisplayColdForecast();
        }
        else
        {
            DisplayDefaultForecast();
        }
    }
    //interpretations of the weather forecast
    private void DisplayHumidAndHotForecast()
    {
        Console.WriteLine("Forecast: Improving weather on the way!");
    }

    private void DisplayColdForecast()
    {
        Console.WriteLine("Forecast: Watchout for cooler, rainy weather!");
    }

    private void DisplayDefaultForecast()
    {
        Console.WriteLine("Forecast: More of the same");
    }
}


// main code
class Program
{
    static void Main()
    {
        //weather data instance
        WeatherData weatherData = new WeatherData();
        //instances of different display agents
        CurrentConditionsDisplay currentConditionsDisplay = new CurrentConditionsDisplay();
        StatisticsDisplay statisticsDisplay = new StatisticsDisplay();
        ForecastDisplay forecastDisplay = new ForecastDisplay();
        //connecting the display agents as observers
        weatherData.RegisterObserver(currentConditionsDisplay);
        weatherData.RegisterObserver(statisticsDisplay);
        weatherData.RegisterObserver(forecastDisplay);

        // Add random weather values to imitate measurements from the weather station
        weatherData.SetMeasurements(80, 65, 30.4f);
        weatherData.SetMeasurements(82, 70, 29.2f);
        weatherData.SetMeasurements(78, 90, 29.2f);

    }
}
