// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import * as authInfo from 'auth_config.json';

export const environment = {
  production: false,
  journeyApi: 'https://localhost:7030/api/',
  auth0:{
    domain: "dev-2mb38pu2.us.auth0.com",
    clientId: "XaJDsh1K9YUMMBTDE0UFtQLYAj86v6nN",
    redirectUri: window.location.origin
  },
  recaptcha:{
    siteKey: "6LcpIQEmAAAAAEOKGPM2SHNlJgcDDcQWZcAmsFMk"
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
