using API._Services.Interfaces;
using API.Accessor._Interfaces;
using API.DTO;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SD3_API.Helpers.Utilities;

namespace API._Services.Services
{
    public class SHomeMain : IHomeMain
    {
        private readonly IRepositoryAccessor _repositoryAccessor;
        public SHomeMain(IRepositoryAccessor repositoryAccessor)
        {
            _repositoryAccessor = repositoryAccessor;
        }
        #region Create        
        public async Task<OperationResult> Create(DataCreate data)
        {
            using var _transaction = await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                var checkDataAfter = await _repositoryAccessor.Information.FindAll().ToListAsync();

                // if (checkDataAfter.Any(x => x.ID == data.DataTable.ID))
                //     return new OperationResult(false, "Trùng dữ liệu");
                var dataInfor = new Information
                {
                    Name = data.DataTable.Name,
                    YearOld = "18",
                };
                _repositoryAccessor.Information.Add(dataInfor);
                await _repositoryAccessor.SaveChangesAsync();

                var maxIDInfor = await _repositoryAccessor.Information.FindAll().MaxAsync(x => x.ID);


                var position = data.DataTable.Position.Split("+");

                var positionID = await _repositoryAccessor.Position.FindAll(x => position.Contains(x.Name)).Select(x => x.ID).ToListAsync();

                var dataPosition = new List<PositionInformation>();
                var dataQualityAfter = new List<QualityAfter>();

                foreach (var item in positionID)
                {
                    dataPosition.Add(new PositionInformation
                    {
                        InforID = maxIDInfor,
                        PositionID = item
                    });
                }

                var dataQualityBefore = new QualityBefore { };
                dataQualityBefore = new QualityBefore
                {
                    InforID = maxIDInfor,
                    CanPha = data.DataTable.CanPha,
                    KemNguoi = data.DataTable.KemNguoi,
                    ChayCho = data.DataTable.ChayCho,
                    DanhDau = data.DataTable.DanhDau,
                    DungCam = data.DataTable.DungCam,
                    ChuyenBong = data.DataTable.ChuyenBong,
                    ReBong = data.DataTable.ReBong,
                    TatCanh = data.DataTable.TatCanh,
                    SutManh = data.DataTable.SutManh,
                    DutDiem = data.DataTable.DutDiem,
                    TheLuc = data.DataTable.TheLuc,
                    SucManh = data.DataTable.SucManh,
                    XongXao = data.DataTable.XongXao,
                    TocDo = data.DataTable.TocDo,
                    SangTao = data.DataTable.SangTao
                };
                _repositoryAccessor.QualityBefore.Add(dataQualityBefore);

                for (int i = 0; i < data.DataAfter.Length; i++)
                {
                    var item = data.DataAfter[i];
                    dataQualityAfter.Add(new QualityAfter
                    {
                        ID = i + 1,
                        InforID = maxIDInfor,
                        PlanID = 1,
                        ExerciseID = item.ExerciseID,
                        Average = item.Average,
                        CanPha = item.CanPha,
                        KemNguoi = item.KemNguoi,
                        ChayCho = item.ChayCho,
                        DanhDau = item.DanhDau,
                        DungCam = item.DungCam,
                        ChuyenBong = item.ChuyenBong,
                        ReBong = item.ReBong,
                        TatCanh = item.TatCanh,
                        SutManh = item.SutManh,
                        DutDiem = item.DutDiem,
                        TheLuc = item.TheLuc,
                        SucManh = item.SucManh,
                        XongXao = item.XongXao,
                        TocDo = item.TocDo,
                        SangTao = item.SangTao
                    });
                }
                var checkDataAfternew = await _repositoryAccessor.Information.FindAll().ToListAsync();

                _repositoryAccessor.PositionInformation.AddMultiple(dataPosition);
                _repositoryAccessor.QualityBefore.Add(dataQualityBefore);
                _repositoryAccessor.QualityAfter.AddMultiple(dataQualityAfter);


                await _repositoryAccessor.SaveChangesAsync();
                await _transaction.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding dataQualityBefore: {ex.Message}");
                await _transaction.RollbackAsync();
                return new OperationResult(false);
            }
        }
        #endregion
        #region Update

        public async Task<OperationResult> Update(DataCreate data)
        {
            using var _transaction = await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                // Xóa table để cập nhật lại
                var delAfter = await _repositoryAccessor.QualityAfter.FindAll(x => x.InforID == data.DataTable.InforID && x.PlanID == data.DataTable.PlanID).ToListAsync();
                var delPosition = await _repositoryAccessor.PositionInformation.FindAll(x => x.InforID == data.DataTable.InforID).ToListAsync();
                _repositoryAccessor.QualityAfter.RemoveMultiple(delAfter);
                _repositoryAccessor.PositionInformation.RemoveMultiple(delPosition);
                await _repositoryAccessor.SaveChangesAsync();

                // Cập nhật ThongTin
                var dataInfor = await _repositoryAccessor.Information.FirstOrDefaultAsync(x => x.ID == data.DataTable.InforID);
                if (dataInfor == null)
                    return new OperationResult(false);
                dataInfor.Name = data.DataTable.Name.Trim();
                dataInfor.YearOld = "18";
                dataInfor.Quality = 0;
                _repositoryAccessor.Information.Update(dataInfor);


                // Chỉnh sửa vị trí
                var position = data.DataTable.Position.Split("+");
                var listPosition = await _repositoryAccessor.Position.FindAll(x => position.Contains(x.Name)).Select(x => x.ID).ToListAsync();
                var dataPosition = new List<PositionInformation>();
                foreach (var item in listPosition)
                {
                    dataPosition.Add(new PositionInformation
                    {
                        InforID = data.DataTable.InforID,
                        PositionID = item
                    });
                }
                _repositoryAccessor.PositionInformation.AddMultiple(dataPosition);

                // Chỉnh sửa Chất lượng Before
                var dataBefore = await _repositoryAccessor.QualityBefore.FirstOrDefaultAsync(x => x.InforID == data.DataTable.InforID);
                if (dataBefore == null)
                    return new OperationResult(false);
                dataBefore.CanPha = data.DataTable.CanPha;
                dataBefore.KemNguoi = data.DataTable.KemNguoi;
                dataBefore.ChayCho = data.DataTable.ChayCho;
                dataBefore.DanhDau = data.DataTable.DanhDau;
                dataBefore.DungCam = data.DataTable.DungCam;
                dataBefore.ChuyenBong = data.DataTable.ChuyenBong;
                dataBefore.ReBong = data.DataTable.ReBong;
                dataBefore.TatCanh = data.DataTable.TatCanh;
                dataBefore.SutManh = data.DataTable.SutManh;
                dataBefore.DutDiem = data.DataTable.DutDiem;
                dataBefore.TheLuc = data.DataTable.TheLuc;
                dataBefore.SucManh = data.DataTable.SucManh;
                dataBefore.XongXao = data.DataTable.XongXao;
                dataBefore.TocDo = data.DataTable.TocDo;
                dataBefore.SangTao = data.DataTable.SangTao;
                _repositoryAccessor.QualityBefore.Update(dataBefore);

                // Chỉnh sửa Chất lượng Affter
                var dataQualityAfter = new List<QualityAfter>();
                if (data.DataAfter.Length != 0)
                {
                    for (int i = 0; i < data.DataAfter.Length; i++)
                    {
                        var item = data.DataAfter[i];
                        dataQualityAfter.Add(new QualityAfter
                        {
                            ID = i + 1,
                            InforID = data.DataTable.InforID,
                            PlanID = data.DataTable.PlanID,
                            ExerciseID = item.ExerciseID,
                            Average = item.Average,
                            CanPha = item.CanPha,
                            KemNguoi = item.KemNguoi,
                            ChayCho = item.ChayCho,
                            DanhDau = item.DanhDau,
                            DungCam = item.DungCam,
                            ChuyenBong = item.ChuyenBong,
                            ReBong = item.ReBong,
                            TatCanh = item.TatCanh,
                            SutManh = item.SutManh,
                            DutDiem = item.DutDiem,
                            TheLuc = item.TheLuc,
                            SucManh = item.SucManh,
                            XongXao = item.XongXao,
                            TocDo = item.TocDo,
                            SangTao = item.SangTao
                        });
                    }
                    _repositoryAccessor.QualityAfter.AddMultiple(dataQualityAfter);
                }

                await _repositoryAccessor.SaveChangesAsync();
                await _transaction.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                await _transaction.RollbackAsync();
                return new OperationResult(false, ex.InnerException);
            }
        }
        #endregion
        public async Task<OperationResult> Delete(int id)
        {
            var dataInfor = await _repositoryAccessor.Information.FirstOrDefaultAsync(x => x.ID == id);
            if (dataInfor != null)
                _repositoryAccessor.Information.Remove(dataInfor);

            var dataPosition = await _repositoryAccessor.PositionInformation.FindAll(x => x.InforID == id).ToListAsync();
            if (dataPosition != null)
                _repositoryAccessor.PositionInformation.RemoveMultiple(dataPosition);

            var dataBefore = await _repositoryAccessor.QualityBefore.FirstOrDefaultAsync(x => x.InforID == id);
            if (dataBefore != null)
                _repositoryAccessor.QualityBefore.Remove(dataBefore);

            var dataAfter = await _repositoryAccessor.QualityAfter.FindAll(x => x.InforID == id).ToListAsync();
            if (dataBefore != null)
                _repositoryAccessor.QualityAfter.RemoveMultiple(dataAfter);

            return new OperationResult(await _repositoryAccessor.SaveChangesAsync());
        }
        #region  GetData
        public async Task<HomeMainDto> GetData(HomeMainParam param)
        {
            var pred = PredicateBuilder.New<QualityBefore>(true);
            if (!string.IsNullOrWhiteSpace(param.InforID.ToString()))
                pred.And(x => x.InforID == param.InforID);

            var data = await _repositoryAccessor.Information.FindAll(x => x.ID == param.InforID).ToListAsync();
            var dataQuality = await _repositoryAccessor.QualityBefore.FindAll(pred).ToListAsync();
            var dataPosition = data
                .GroupJoin(_repositoryAccessor.PositionInformation.FindAll(),
                    x => x.ID,
                    y => y.InforID,
                    (x, y) => new { Infor = x, PositionInfor = y })
                .SelectMany(x => x.PositionInfor.DefaultIfEmpty(),
                    (x, y) => new { x.Infor, PositionInfor = y })
                .GroupJoin(_repositoryAccessor.Position.FindAll(),
                    x => x.PositionInfor.PositionID,
                    y => y.ID,
                    (x, y) => new { x.Infor, x.PositionInfor, Position = y })
                .SelectMany(x => x.Position.DefaultIfEmpty(),
                    (x, y) => new { x.Infor, x.PositionInfor, Position = y })
                .Select(x => x.Position.Name).ToList();

            var dataAfter = await _repositoryAccessor.QualityAfter.FindAll(x => x.InforID == param.InforID)
                .Select(x => new Quality
                {
                    ID = x.ID,
                    InforID = x.InforID,
                    PlanID = x.PlanID,
                    ExerciseID = x.ExerciseID,
                    Average = x.Average ?? 0,
                    CanPha = x.CanPha,
                    KemNguoi = x.KemNguoi,
                    ChayCho = x.ChayCho,
                    DanhDau = x.DanhDau,
                    DungCam = x.DungCam,
                    ChuyenBong = x.ChuyenBong,
                    ReBong = x.ReBong,
                    TatCanh = x.TatCanh,
                    SutManh = x.SutManh,
                    DutDiem = x.DutDiem,
                    TheLuc = x.TheLuc,
                    SucManh = x.SucManh,
                    XongXao = x.XongXao,
                    TocDo = x.TocDo,
                    SangTao = x.SangTao,
                })
                .OrderBy(x => x.PlanID)
                .ThenBy(x => x.ID)
                .ToListAsync();



            var result = dataQuality.Join(data,
                    x => x.InforID,
                    y => y.ID,
                    (x, y) => new { Quality = x, Infor = y })
                .Select(x => new HomeMainDto
                {
                    Name = x.Infor.Name,
                    Position = string.Join("+", dataPosition),
                    Personality = "",
                    CanPha = x.Quality.CanPha,
                    KemNguoi = x.Quality.KemNguoi,
                    ChayCho = x.Quality.ChayCho,
                    DanhDau = x.Quality.DanhDau,
                    DungCam = x.Quality.DungCam,
                    ChuyenBong = x.Quality.ChuyenBong,
                    ReBong = x.Quality.ReBong,
                    TatCanh = x.Quality.TatCanh,
                    SutManh = x.Quality.SutManh,
                    DutDiem = x.Quality.DutDiem,
                    TheLuc = x.Quality.TheLuc,
                    SucManh = x.Quality.SucManh,
                    XongXao = x.Quality.XongXao,
                    TocDo = x.Quality.TocDo,
                    SangTao = x.Quality.SangTao,
                    QualityAfter = dataAfter ?? new List<Quality>()
                })
                .FirstOrDefault();
            return result;
        }
        #endregion
        #region So sánh GetListCompares
        public async Task<List<Quality>> GetListCompares(int inforID)
        {
            var data = await _repositoryAccessor.QualityAfter.FindAll(x => x.InforID == inforID).ToListAsync();
            if (data == null)
                return null;

            var groupedData = data
                .GroupBy(x => x.PlanID)
                .Select(g => g.OrderByDescending(x => x.ID).First())
                .ToList();

            return groupedData.Select(x => new Quality
            {
                InforID = x.InforID,
                PlanID = x.PlanID,
                // ChatLuongChung = x.ChatLuong ?? 0,
                CanPha = x.CanPha,
                KemNguoi = x.KemNguoi,
                ChayCho = x.ChayCho,
                DanhDau = x.DanhDau,
                DungCam = x.DungCam,
                ChuyenBong = x.ChuyenBong,
                ReBong = x.ReBong,
                TatCanh = x.TatCanh,
                SutManh = x.SutManh,
                DutDiem = x.DutDiem,
                TheLuc = x.TheLuc,
                SucManh = x.SucManh,
                XongXao = x.XongXao,
                TocDo = x.TocDo,
                SangTao = x.SangTao
            }).OrderBy(x => x.PlanID).ToList();
        }
        #endregion
        #region Create Compares
        public async Task<OperationResult> CreateCompare(DataCreate data)
        {
            var maxPlanID = await _repositoryAccessor.QualityAfter
                  .FindAll(x => x.InforID == data.DataTable.InforID)
                  .MaxAsync(x => (int?)x.PlanID) ?? 0;
            var dataQualityAfter = new List<QualityAfter>();
            for (int i = 0; i < data.DataAfter.Length; i++)
            {
                var item = data.DataAfter[i];
                dataQualityAfter.Add(new QualityAfter
                {
                    ID = i + 1,
                    InforID = data.DataTable.InforID,
                    PlanID = maxPlanID + 1,
                    ExerciseID = item.ExerciseID,
                    Average = item.Average,
                    CanPha = item.CanPha,
                    KemNguoi = item.KemNguoi,
                    ChayCho = item.ChayCho,
                    DanhDau = item.DanhDau,
                    DungCam = item.DungCam,
                    ChuyenBong = item.ChuyenBong,
                    ReBong = item.ReBong,
                    TatCanh = item.TatCanh,
                    SutManh = item.SutManh,
                    DutDiem = item.DutDiem,
                    TheLuc = item.TheLuc,
                    SucManh = item.SucManh,
                    XongXao = item.XongXao,
                    TocDo = item.TocDo,
                    SangTao = item.SangTao
                });
            }
            _repositoryAccessor.QualityAfter.AddMultiple(dataQualityAfter);
            return new OperationResult(await _repositoryAccessor.SaveChangesAsync());
        }

        public async Task<OperationResult> DeleteCompare(Quality data)
        {
            var dataCheck = await _repositoryAccessor.QualityAfter.FirstOrDefaultAsync(x => x.InforID == data.InforID && x.PlanID == data.PlanID);
            if (data == null)
                return null;
            _repositoryAccessor.QualityAfter.Remove(dataCheck);
            return new OperationResult(await _repositoryAccessor.SaveChangesAsync());

        }
        #endregion

        public async Task<List<KeyValuePair<int, string>>> GetListExercise()
        {
            var data = await _repositoryAccessor.Exercises.FindAll().ToListAsync();
            return data.Select(x => new KeyValuePair<int, string>(x.ID, x.Name)).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPlayers()
        {
            var data = await _repositoryAccessor.Information.FindAll().ToListAsync();
            return data.Select(x => new KeyValuePair<string, string>(x.ID.ToString(), x.Name)).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetKeys()
        {
            var data = await _repositoryAccessor.MainAttributes.FindAll().ToListAsync();
            return data.Select(x => new KeyValuePair<string, string>(x.Name, x.Type.ToString())).ToList();
        }
        #region GetListAttribute
        public async Task<List<KeyValuePair<string, string>>> GetListThuocTinh(int ExerciseID, string Position)
        {
            var data = await _repositoryAccessor.Exercises.FindAll(x => x.ID == ExerciseID).ToListAsync();
            var result = data
                .GroupJoin(_repositoryAccessor.ExerciseAttributes.FindAll(x => x.ExerciseID == ExerciseID),
                    x => x.ID,
                    y => y.ExerciseID,
                    (x, y) => new { Exercise = x, ExerciseAttributes = y })
                .SelectMany(x => x.ExerciseAttributes.DefaultIfEmpty(),
                    (x, y) => new { x.Exercise, ExerciseAttributes = y })
                .GroupJoin(_repositoryAccessor.MainAttributes.FindAll(),
                    x => x.ExerciseAttributes.AttributeID,
                    y => y.ID,
                    (x, y) => new { x.Exercise, x.ExerciseAttributes, MainAttributes = y })
                .SelectMany(x => x.MainAttributes.DefaultIfEmpty(),
                    (x, y) => new { x.Exercise, x.ExerciseAttributes, MainAttributes = y })
                .Select(x => x.MainAttributes.Name)
                .ToList();



            var dataDisabled = await GetListDisable(Position);

            var filteredResult = dataDisabled
                .Where(x => result.Contains(x.Key))
                .ToList();


            return filteredResult;
        }
        #endregion

        public async Task<List<KeyValuePair<string, string>>> GetListDisable(string Position)
        {
            var listPosition = Position.Split("+");
            var dataPosition = await _repositoryAccessor.Position.FindAll(x => listPosition.Contains(x.Name.Trim())).Select(x => x.ID).ToListAsync();

            var result = _repositoryAccessor.TypeAttributes
                .FindAll(x => dataPosition.Contains(x.PositionID))
                .Join(_repositoryAccessor.MainAttributes.FindAll(),
                    x => x.AttributeID,
                    y => y.ID,
                    (x, y) => new { TypeAttributes = x, MainAttributes = y })
                .GroupBy(x => x.TypeAttributes.AttributeID)
                .Select(g => new
                {
                    ID = g.Key,
                    Name = g.Select(x => x.MainAttributes.Name).FirstOrDefault(),
                    KQ = g.Max(x => Convert.ToInt32(x.TypeAttributes.Disable)) == 0 ? 0 : 1
                })
                .ToList();
            return result.Select(x => new KeyValuePair<string, string>(x.Name, x.KQ.ToString())).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetExercisesForAttributes(string Key)
        {
            var data = await _repositoryAccessor.MainAttributes.FindAll(x => x.Name == Key).ToListAsync();
            var result = data
                .GroupJoin(_repositoryAccessor.ExerciseAttributes.FindAll(),
                    x => x.ID,
                    y => y.AttributeID,
                    (x, y) => new { MainAttributes = x, ExerciseAttributes = y })
                .SelectMany(x => x.ExerciseAttributes.DefaultIfEmpty(),
                    (x, y) => new { x.MainAttributes, ExerciseAttributes = y })
                .GroupJoin(_repositoryAccessor.Exercises.FindAll(),
                    x => x.ExerciseAttributes.ExerciseID,
                    y => y.ID,
                    (x, y) => new { x.MainAttributes, x.ExerciseAttributes, Exercises = y })
                .SelectMany(x => x.Exercises.DefaultIfEmpty(),
                    (x, y) => new { x.MainAttributes, x.ExerciseAttributes, Exercises = y })
                .Select(x => new KeyValuePair<string, string>(x.Exercises.ID.ToString(), x.Exercises.Name))
                .ToList();


            return result;
        }
    }
}