using DonkeyLive.Shared.Domain;
using DonkeyLive.WebApi.Controllers.Base;
using DonkeyLive.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DonkeyLive.WebApi.Controllers;

public class LiveGroupController:ApiControllerBase
{
    private readonly UnitOfWork _unitOfWork;

    public LiveGroupController(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult List()
    {
        var groups = _unitOfWork.Query<LiveGroup>().ToList();
        return Success(groups);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var group = _unitOfWork.Query<LiveGroup>().FirstOrDefault(x => x.Id == id);
        if (group == null)
        {
            return Error("Group not found");
        }

        return Success(group);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LiveGroup group)
    {
        _unitOfWork.Add(group);
        await _unitOfWork.SaveChangesAsync();
        return Success("Group created", group);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] LiveGroup group)
    {
        var existingGroup = _unitOfWork.Query<LiveGroup>().FirstOrDefault(x => x.Id == id);
        if (existingGroup == null)
        {
            return Error("Group not found");
        }

        existingGroup.Name = group.Name;
        existingGroup.Description = group.Description;
        existingGroup.Tags = group.Tags;

        await _unitOfWork.SaveChangesAsync();
        return Success("Group updated", existingGroup);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingGroup = _unitOfWork.Query<LiveGroup>().FirstOrDefault(x => x.Id == id);
        if (existingGroup == null)
        {
            return Error("Group not found");
        }

        _unitOfWork.Delete(existingGroup);
        await _unitOfWork.SaveChangesAsync();
        return Success("Group deleted");
    }
}
