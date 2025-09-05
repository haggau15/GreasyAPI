using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public sealed class PlaceDocument
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("greasereviews")]
    public string GreaseReviews { get; set; }

    [JsonPropertyName("types")]
    public List<string> Types { get; set; }

    [JsonPropertyName("formattedAddress")]
    public string FormattedAddress { get; set; }

    [JsonPropertyName("location")]
    public GeoLocation Location { get; set; }

    [JsonPropertyName("displayName")]
    public LocalizedText DisplayName { get; set; }

    // Container with its own metadata + an array of review items
    [JsonPropertyName("reviews")]
    public ReviewsSection Reviews { get; set; }

    [JsonPropertyName("averagesentiment")]
    public Sentiment AverageSentiment { get; set; }
}

public sealed class GeoLocation
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}

public sealed class LocalizedText
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("languageCode")]
    public string LanguageCode { get; set; }
}

public sealed class ReviewsSection
{
    // Note: this section repeats place metadata + holds the "reviews" array.

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("types")]
    public List<string> Types { get; set; }

    [JsonPropertyName("formattedAddress")]
    public string FormattedAddress { get; set; }

    [JsonPropertyName("location")]
    public GeoLocation Location { get; set; }

    [JsonPropertyName("displayName")]
    public LocalizedText DisplayName { get; set; }

    // The actual list of review entries
    [JsonPropertyName("reviews")]
    public List<Review> Items { get; set; }
}

public sealed class Review
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("relativePublishTimeDescription")]
    public string RelativePublishTimeDescription { get; set; }

    [JsonPropertyName("rating")]
    public int Rating { get; set; }

    // "text" includes sentiment
    [JsonPropertyName("text")]
    public ReviewText Text { get; set; }

    // "originalText" has no sentiment
    [JsonPropertyName("originalText")]
    public LocalizedText OriginalText { get; set; }

    [JsonPropertyName("authorAttribution")]
    public AuthorAttribution AuthorAttribution { get; set; }

    [JsonPropertyName("publishTime")]
    public DateTimeOffset PublishTime { get; set; }

    [JsonPropertyName("flagContentUri")]
    public string FlagContentUri { get; set; }

    [JsonPropertyName("googleMapsUri")]
    public string GoogleMapsUri { get; set; }
}

public sealed class ReviewText
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("languageCode")]
    public string LanguageCode { get; set; }

    [JsonPropertyName("sentiment")]
    public Sentiment Sentiment { get; set; }
}

public sealed class AuthorAttribution
{
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }

    [JsonPropertyName("photoUri")]
    public string PhotoUri { get; set; }
}

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public sealed class Sentiment
{
    // Numbers come as strings in your JSON; this attribute lets them be parsed.
    [JsonPropertyName("Positive")]
    public decimal Positive { get; set; }

    [JsonPropertyName("Negative")]
    public decimal Negative { get; set; }

    [JsonPropertyName("Neutral")]
    public decimal Neutral { get; set; }
}
