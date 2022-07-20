using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace MicrosoftEntra.VerifiedId.Client;

public static class VerifiedIdServiceCollectionExtensions
{
    #region Issuance

    public static IServiceCollection AddVerifiedIdIssuance(this IServiceCollection services, IConfiguration namedConfigurationSection)
    {
        var issuanceRequestClientOptions = namedConfigurationSection.Get<IssuanceRequestClientOptions>();
        var msalOptions = namedConfigurationSection.Get<ConfidentialClientApplicationOptions>();
        return services.AddVerifiedIdIssuance(issuanceRequestClientOptions, msalOptions);
    }

    public static IServiceCollection AddVerifiedIdIssuance(this IServiceCollection services, Action<IssuanceRequestClientOptions> configureIssuanceRequestClientOptions, Action<ConfidentialClientApplicationOptions> configureMsalOptions)
    {
        var issuanceRequestClientOptions = new IssuanceRequestClientOptions();
        configureIssuanceRequestClientOptions.Invoke(issuanceRequestClientOptions);
        var msalOptions = new ConfidentialClientApplicationOptions();
        configureMsalOptions.Invoke(msalOptions);
        return services.AddVerifiedIdIssuance(issuanceRequestClientOptions, msalOptions);
    }

    public static IServiceCollection AddVerifiedIdIssuance(this IServiceCollection services, IssuanceRequestClientOptions issuanceRequestClientOptions, ConfidentialClientApplicationOptions msalOptions)
    {
        // Add Verified ID Issuance services.
        AddCommonRequestServices(services, msalOptions);
        services.AddSingleton<IssuanceRequestClientOptions>(issuanceRequestClientOptions);
        services.AddScoped<IssuanceRequestClient>();
        return services;
    }

    #endregion

    #region Presentation

    public static IServiceCollection AddVerifiedIdPresentation(this IServiceCollection services, IConfiguration namedConfigurationSection)
    {
        var issuanceRequestClientOptions = namedConfigurationSection.Get<PresentationRequestClientOptions>();
        var msalOptions = namedConfigurationSection.Get<ConfidentialClientApplicationOptions>();
        return services.AddVerifiedIdPresentation(issuanceRequestClientOptions, msalOptions);
    }

    public static IServiceCollection AddVerifiedIdPresentation(this IServiceCollection services, Action<PresentationRequestClientOptions> configureIssuanceRequestClientOptions, Action<ConfidentialClientApplicationOptions> configureMsalOptions)
    {
        var issuanceRequestClientOptions = new PresentationRequestClientOptions();
        configureIssuanceRequestClientOptions.Invoke(issuanceRequestClientOptions);
        var msalOptions = new ConfidentialClientApplicationOptions();
        configureMsalOptions.Invoke(msalOptions);
        return services.AddVerifiedIdPresentation(issuanceRequestClientOptions, msalOptions);
    }

    public static IServiceCollection AddVerifiedIdPresentation(this IServiceCollection services, PresentationRequestClientOptions presentationRequestClientOptions, ConfidentialClientApplicationOptions msalOptions)
    {
        // Add Verified ID Presentation services.
        AddCommonRequestServices(services, msalOptions);
        services.AddSingleton<PresentationRequestClientOptions>(presentationRequestClientOptions);
        services.AddScoped<PresentationRequestClient>();
        return services;
    }

    #endregion

    #region Well-Known Endpoints

    public static IServiceCollection AddVerifiedIdWellKnownEndpoints(this IServiceCollection services, IConfiguration namedConfigurationSection)
    {
        var wellKnownEndpointOptions = namedConfigurationSection.Get<WellKnownEndpointOptions>();
        return services.AddVerifiedIdWellKnownEndpoints(wellKnownEndpointOptions);
    }

    public static IServiceCollection AddVerifiedIdWellKnownEndpoints(this IServiceCollection services, Action<WellKnownEndpointOptions> configureWellKnownEndpointOptions)
    {
        var wellKnownEndpointOptions = new WellKnownEndpointOptions();
        configureWellKnownEndpointOptions.Invoke(wellKnownEndpointOptions);
        return services.AddVerifiedIdWellKnownEndpoints(wellKnownEndpointOptions);
    }

    public static IServiceCollection AddVerifiedIdWellKnownEndpoints(this IServiceCollection services, WellKnownEndpointOptions wellKnownEndpointOptions)
    {
        services.AddSingleton<WellKnownEndpointOptions>(wellKnownEndpointOptions);
        return services;
    }

    #endregion

    #region Common

    private static void AddCommonRequestServices(IServiceCollection services, ConfidentialClientApplicationOptions msalOptions)
    {
        // Add HTTP client services.
        services.AddHttpClient();

        // Add MSAL services.
        var confidentialClientApplication = ConfidentialClientApplicationBuilder.CreateWithApplicationOptions(msalOptions).Build();
        services.AddSingleton<IConfidentialClientApplication>(confidentialClientApplication);
    }

    #endregion
}