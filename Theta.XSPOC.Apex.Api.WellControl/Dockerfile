#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN useradd xspoc -d /home/xspoc
USER xspoc:xspoc
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Theta.XSPOC.Apex.Api.WellControl/Theta.XSPOC.Apex.Api.WellControl.csproj"
RUN dotnet build "Theta.XSPOC.Apex.Api.WellControl/Theta.XSPOC.Apex.Api.WellControl.csproj" -c Release -o /app/build
RUN useradd xspoc -d /home/xspoc 
RUN mkdir /home/xspoc
USER xspoc:xspoc

FROM build AS publish
USER root:root
RUN dotnet publish "Theta.XSPOC.Apex.Api.WellControl/Theta.XSPOC.Apex.Api.WellControl.csproj" -c Release -o /app/publish
USER xspoc:xspoc

FROM base AS final
USER root:root
WORKDIR /app
COPY --from=publish /app/publish .
RUN chown -R xspoc .
USER xspoc:xspoc
ENTRYPOINT ["dotnet", "Theta.XSPOC.Apex.Api.WellControl.dll"]