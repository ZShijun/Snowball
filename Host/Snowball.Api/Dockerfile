FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT Production
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_URLS="https://+;http://+"

ENV ASPNETCORE_Kestrel__Certificates__Default__Password Gx0&hdu\$PHJI3QiU
ENV ASPNETCORE_Kestrel__Certificates__Default__Path /https/aspnetapp.pfx

COPY . .
ENTRYPOINT ["dotnet", "Snowball.Api.dll"]