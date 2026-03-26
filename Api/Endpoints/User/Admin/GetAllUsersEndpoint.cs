using Api.Endpoints.Helpers;
using Application.Features.Users.Admin.GetAll;
using Domain.Specifications.Sorts;
using FastEndpoints;
using Shared.Results;

namespace Api.Endpoints.User.Admin;

public class GetAllUsersEndpoint : Endpoint<GetAllUsersRequest, Result<ResponseListBase<GetAllUserResponse>>>
{
    public override void Configure()
    {
        Get("/admin/users");

        Summary(s =>
        {
            s.Summary = "Obtener todos los usuarios";
            s.Description = "Retorna un listado paginado de usuarios con opción de ordenar por campos permitidos. Solo accesible por administradores.";
            s.ExampleRequest = new GetAllUsersRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Descending = true,
                SortBy = UserSort.CreatedAt
            };
            s.Response<Result<ResponseListBase<GetAllUserResponse>>>(200, "Listado de usuarios obtenido exitosamente");
            s.Response(400, "Request inválido (paginación o sorting incorrecto)");
            s.Response(401, "No autorizado");
            s.Response(403, "Acceso denegado: se requiere rol de administrador");
        });
    }

    public override async Task HandleAsync(GetAllUsersRequest req, CancellationToken ct)
    {
        await EndpointHelper.HandleAsync(
        req,
        new GetAllUsersRequestValidator(),
        async () =>
        {
            var command = new GetAllUsersQuery { Request = req };
            return await command.ExecuteAsync(ct);
        },
        sendResponse: (obj, statusCode) => Send.ResultAsync(Results.Json(obj, statusCode: statusCode)),
        sendOk: obj => Send.OkAsync((Result<ResponseListBase<GetAllUserResponse>>)obj),
        ct);
    }
}
