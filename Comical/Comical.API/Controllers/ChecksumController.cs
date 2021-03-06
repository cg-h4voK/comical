﻿using Comical.Models.Enums;
using Comical.Models.Http;
using Comical.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Comical.API.Controllers
{
    public class ChecksumController : ApiController
    {
        public ChecksumRecalculationResponse Post(ChecksumRecalculationRequest body)
        {
            if (body == null)
                return "Parámetro 'body' no puede ser nulo.";

            if (String.IsNullOrWhiteSpace(body.UserName))
                return "El usuario no puede ser vacío.";

            if (String.IsNullOrWhiteSpace(body.Password))
                return "La contraseña no puede ser vacía.";

            var authenticationService = new AuthenticationService();
            var authenticationResponse = authenticationService.Authenticate(body.UserName, body.Password);

            if (!authenticationResponse.Authenticated)
                return authenticationResponse.ValidationError;

            var isGranted = AuthorizationService.obj.IsEnabledFor(authenticationResponse.UserId, PermissionCodes.VerifierDigits_CanFix);

            if (!isGranted)
                return "No tiene permisos para recalcular los dígitos verificadores.";

            try
            {
                DatabaseStatusService.obj.KillAllConnections();

                DatabaseStatusService.obj.SetUnderMaintenance();
                DatabaseStatusService.obj.SetHasChecksumError();

                ChecksumService.obj.ResetHorizontalVerifiers();
                ChecksumService.obj.ResetVerticalVerifiers();

                DatabaseStatusService.obj.UnsetUnderMaintenance();
                DatabaseStatusService.obj.UnsetHasChecksumError();
            }
            catch(Exception ex)
            {
                LoggingService.obj.Log("ChecksumController", ex);
                return "Ha ocurrido un error en la ejecución. Consulte la bitácora.";
            }

            return new ChecksumRecalculationResponse();
        }
    }
}
