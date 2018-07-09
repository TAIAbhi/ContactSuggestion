using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using DAL;
namespace BAL
{
    public class UserDetails
    {
        public UserDetails()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string MobileFormat(string str)
        {
            str = str.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "");
            str = 10 < str.Length ? str.Substring(str.Length - 10) : str;
            return str;
        }

        public bool SaveUsers(string mobile, string email, string pwd, string firstName, string lastName, string source)
        {

            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spSaveUserDetails";
                SqlParameter[] parameters = new SqlParameter[7];

                parameters[0] = new SqlParameter("@Mobile", mobile);
                parameters[1] = new SqlParameter("@Email", email);
                parameters[2] = new SqlParameter("@Password", pwd);
                parameters[3] = new SqlParameter("@FirstName", firstName);
                parameters[4] = new SqlParameter("@LastName", lastName);
                parameters[5] = new SqlParameter("@Source", source);
                parameters[6] = new SqlParameter("@IsSaved", outParamSave);
                parameters[6].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[6].Value == null ? 0 : Convert.ToInt32(parameters[6].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public string IsUserExists(string mobile, string emailIdAccess)
        {
            string result = string.Empty;
            int noOfEffectedRows = 0;
            try
            {
                string spName = "spCheck_DuplicateUserDetails";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@Mobile", mobile);
                parameters[1] = new SqlParameter("@Email", emailIdAccess);
                parameters[2] = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 100, result);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                result = parameters[2].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool SaveUserDetails(string userXmlData, string source, int userId)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spUser_UserDetailsSave";
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@ImportData", userXmlData);
                parameters[1] = new SqlParameter("@ExistingUserID", userId);
                parameters[2] = new SqlParameter("@Source", source);
                parameters[3] = new SqlParameter("@IsSaved", outParamSave);
                parameters[3].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[3].Value == null ? 0 : Convert.ToInt32(parameters[3].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable GetSourceDetails(string mobile, string password, out int role)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetSourceDetails";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@Mobile", mobile);
                parameters[1] = new SqlParameter("@Password", password);
                parameters[2] = new SqlParameter("@IsAdmin", 0);
                parameters[2].Direction = ParameterDirection.Output;
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }
                role = Convert.ToInt32(parameters[2].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public DataSet GetCategory(int? catid, int? contactId)
        {
            DataSet dtuserDtails = new DataSet();

            try
            {
                string spName = "spGetCategory";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CatId", catid);
                parameters[1] = new SqlParameter("@ContactId", contactId);
                parameters[2] = new SqlParameter("@IsRequest",DBNull.Value);

                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public DataSet GetSuggestion(int? catId, int? subCatId, int? sugId, int? contactId, string token, int? sourceId)
        {
            DataSet dtuserDtails = new DataSet();

            try
            {

                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@CatId", catId);
                parameters[1] = new SqlParameter("@SubCatId", subCatId);
                parameters[2] = new SqlParameter("@ContactId", contactId);
                parameters[3] = new SqlParameter("@SugId", sugId);

                if (!string.IsNullOrEmpty(token))
                {
                    parameters[4] = new SqlParameter("@Token", token);
                }
                else
                {
                    parameters[4] = new SqlParameter("@Token", DBNull.Value);
                }
                parameters[5] = new SqlParameter("@SourceId", sourceId);
                string spName = "spGetSuggestion";

                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataSet GetRequestSuggestion(int? catId, int? subCatId, int? contactId, string location, int? microcate, int pageSize, int pageNumber, out int returnTotalRows, int? cityId)
        {


            DataSet dtuserDtails = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[15];
                parameters[0] = new SqlParameter("@CatId", catId);
                parameters[1] = new SqlParameter("@SubCatId", subCatId);
                parameters[2] = new SqlParameter("@ContactId", contactId);
                parameters[3] = new SqlParameter("@SugId", DBNull.Value);



             
                    parameters[4] = new SqlParameter("@Token", DBNull.Value);
                
                parameters[5] = new SqlParameter("@SourceId", DBNull.Value);

                parameters[6] = new SqlParameter("@BusinessName", DBNull.Value);

                if (location == "")
                {
                    parameters[7] = new SqlParameter("@Location", DBNull.Value);
                }
                else
                {

                    parameters[7] = new SqlParameter("@Location", location);
                }
                parameters[8] = new SqlParameter("@IsLocal", DBNull.Value);
                parameters[9] = new SqlParameter("@Microcategory", microcate);

                parameters[10] = new SqlParameter("@ReturnTotalRows", 0);
                parameters[10].Direction = ParameterDirection.Output;

                parameters[11] = new SqlParameter("@PageSize", pageSize);
                parameters[12] = new SqlParameter("@PageNumber", pageNumber);

                // parameters[13] = new SqlParameter("@MCName", microName);

                parameters[13] = new SqlParameter("@MCName", DBNull.Value);

                parameters[14] = new SqlParameter("@CityId", cityId);
                string spName = "spGetRequestSuggestion";

                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                returnTotalRows = parameters[10].Value == null ? 0 : Convert.ToInt32(parameters[10].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public DataSet GetSuggestionFilters(int? catId, int? subCatId, int? sugId, int? contactId, string token, int? sourceId, string businessName,bool ? isLocal, string location, int ? microcate)
        {


           DataSet dtuserDtails = new DataSet();

            try
            {

                SqlParameter[] parameters = new SqlParameter[10];
                parameters[0] = new SqlParameter("@CatId", catId);
                parameters[1] = new SqlParameter("@SubCatId", subCatId);
                parameters[2] = new SqlParameter("@ContactId", contactId);
                parameters[3] = new SqlParameter("@SugId", sugId);

      

                if (!string.IsNullOrEmpty(token))
                {
                    parameters[4] = new SqlParameter("@Token", token);
                }
                else
                {
                    parameters[4] = new SqlParameter("@Token", DBNull.Value);
                }
                parameters[5] = new SqlParameter("@SourceId", sourceId);

                if (!string.IsNullOrEmpty(businessName))
                {
                    parameters[6] = new SqlParameter("@BusinessName", businessName);
                }
                else
                {
                    parameters[6] = new SqlParameter("@BusinessName", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(location))
                {
                    parameters[7] = new SqlParameter("@Location", location);
                }
                else
                {
                    parameters[7] = new SqlParameter("@Location", DBNull.Value);
                }
                parameters[8] = new SqlParameter("@IsLocal", isLocal);
                parameters[9] = new SqlParameter("@Microcategory", microcate);
                string spName = "spGetSuggestionFilters";

                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }


        public DataTable GetMicroCategory(int? Subcatid, int? micro)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetMicroCategory";
                SqlParameter[] parameters = new SqlParameter[2];
                if (Subcatid == 0)
                {
                    parameters[0] = new SqlParameter("@SubCateId", DBNull.Value);

                }
                else
                {
                    parameters[0] = new SqlParameter("@SubCateId", Subcatid);
                }

                parameters[1] = new SqlParameter("@MicroId", micro);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public DataTable GetMySuggestionCategoryWise(string contactNumber)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetMySuggestionCategoryWise";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ContactNumber", contactNumber);
                parameters[1] = new SqlParameter("@Token", DBNull.Value);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetMySuggestionSubCategoryWise(int catId, string contactNumber)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetMySuggestionSubCategoryWise";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CatId", catId);
                parameters[1] = new SqlParameter("@ContactNumber", contactNumber);
                parameters[2] = new SqlParameter("@Token", DBNull.Value);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetAllSuggestionCategoryWise()
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetAllSuggestionCategoryWise";             
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetAllSuggestionSubCategoryWise(int catId)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetAllSuggestionSubCategoryWise";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CatId", catId);
             
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetCategoryCount(int? sourceId)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetCategoryCount";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@SourceId", sourceId);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetContacts(int? contactId, int? sourceId, string contactNumber)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetContacts";
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@ContactId", contactId);
                parameters[1] = new SqlParameter("@SourceId", sourceId);
                parameters[2] = new SqlParameter("@Token", DBNull.Value);
                parameters[3] = new SqlParameter("@Name", DBNull.Value);
                if(!string.IsNullOrEmpty(contactNumber))
                parameters[4] = new SqlParameter("@ContactNumber", contactNumber);
                else
                 parameters[4] = new SqlParameter("@ContactNumber", DBNull.Value);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetLocation(int? locationid, string suburb, string location, int cityId)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetLocation";
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@LocationId", locationid);
                if (suburb == string.Empty)
                {
                    parameters[1] = new SqlParameter("@Suburb", DBNull.Value);
                }
                else
                {
                    parameters[1] = new SqlParameter("@Suburb", suburb);
                }
                if (location == string.Empty)
                {
                    parameters[2] = new SqlParameter("@Location", DBNull.Value);
                }
                else
                {
                    parameters[2] = new SqlParameter("@Location", location);
                }
                if(cityId==0)
                {
                    parameters[3] = new SqlParameter("@CityId", DBNull.Value);
                }
                else
                {
                    parameters[3] = new SqlParameter("@CityId", cityId);
                }
                // parameters[3] = new SqlParameter("@CityId", 1);
                parameters[4] = new SqlParameter("@AreaShortCode", DBNull.Value);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        
       public DataTable GetSubCategorName(int? subCateId)
        {
            DataTable dtuserDtails = new DataTable();
            try
            {
                string spName = "spGetSubCategorName";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@SubCatId", subCateId);
            
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetRanking()
        {
            DataTable dtuserDtails = new DataTable();
            try
            {
                string spName = "spGetRanking";
              
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetSoruce(int? sourceID, bool? isInters)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetSoruce";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@SourceId", sourceID);
                parameters[1] = new SqlParameter("@IsInters", isInters);
                parameters[2] = new SqlParameter("@Name", DBNull.Value);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataSet GetSubCategory(int? catid, int? subcatId, int? contactId)
        {
            DataSet dtuserDtails = new DataSet();


            try
            {
                string spName = "spGetSubCategory";
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@CatId", catid);
                parameters[1] = new SqlParameter("@SubCatId", subcatId);
                parameters[2] = new SqlParameter("@ContactId", contactId);
                parameters[3] = new SqlParameter("@IsRequest",DBNull.Value);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }


        public bool UpdateContact(int contactid, string loc1, string loc2, string loc3, string comments, int understanding, int notification,bool ? isContactDetailsAdded)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spUpdateContact";
                SqlParameter[] parameters = new SqlParameter[9];
                parameters[0] = new SqlParameter("@ContactId", contactid);
                parameters[1] = new SqlParameter("@LocationId1", loc1);
                parameters[2] = new SqlParameter("@LocationId2", loc2);
                parameters[3] = new SqlParameter("@LocationId3", loc3);
                parameters[4] = new SqlParameter("@Comments", comments);
                parameters[5] = new SqlParameter("@ContactLevelUnderstanding", understanding);
                parameters[6] = new SqlParameter("@Notification", notification);
                parameters[7] = new SqlParameter("@IsSaved", outParamSave);
                parameters[7].Direction = ParameterDirection.Output;
                parameters[8] = new SqlParameter("@IsContactDetailsAdded", isContactDetailsAdded);
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[7].Value == null ? 0 : Convert.ToInt32(parameters[7].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdateSkipVedio(int contactid, bool isSkip)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spUpdateSkipVideo";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ContactId", contactid);
                parameters[1] = new SqlParameter("@IsSkipVideo", isSkip);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdateEveryVideoCheck(int contactid, bool isSkip)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spUpdateEveryVideoCheck";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ContactId", contactid);
                parameters[1] = new SqlParameter("@EveryVideoCheck", isSkip);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdateLoginCount(int contactid, string macID)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spUpdateLoginCount";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ContactId", contactid);
                if (macID == string.Empty)
                {
                    parameters[1] = new SqlParameter("@MacID", DBNull.Value);
                }
                else
                {
                    parameters[1] = new SqlParameter("@MacID", macID);
                }
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool SaveContact(int sourceid, string name, string number, string loc1, string loc2, string loc3, string comments, int understanding, int notification,bool ? isContactDetailsAdded)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spSaveContact";
                SqlParameter[] parameters = new SqlParameter[12];
                parameters[0] = new SqlParameter("@SourceId", sourceid);
                parameters[1] = new SqlParameter("@ContactName", name);
                parameters[2] = new SqlParameter("@ContactNumber", number);
                parameters[3] = new SqlParameter("@LocationId1", loc1);
                parameters[4] = new SqlParameter("@LocationId2", loc2);

                parameters[5] = new SqlParameter("@LocationId3", loc3);
                parameters[6] = new SqlParameter("@Comments", comments);

                parameters[7] = new SqlParameter("@ContactLevelUnderstanding", understanding);
                parameters[8] = new SqlParameter("@Notification", notification);


                parameters[9] = new SqlParameter("@IsSaved", outParamSave);
                parameters[9].Direction = ParameterDirection.Output;

                parameters[10] = new SqlParameter("@IsContactDetailsAdded", isContactDetailsAdded);
                parameters[11] = new SqlParameter("@Platform",3);

                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[9].Value == null ? 0 : Convert.ToInt32(parameters[9].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool SaveWebsiteContact(int sourceid, string Firstname,string LastName, string number, string loc1, string loc2, string loc3, string comments, int understanding, int notification, bool? isContactDetailsAdded, string email, string pwd)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
               
                string spName = "spSaveWebSiteContact";
                SqlParameter[] parameters = new SqlParameter[14];
                parameters[0] = new SqlParameter("@SourceId", sourceid);
                parameters[1] = new SqlParameter("@FirstName", Firstname);
                parameters[2] = new SqlParameter("@ContactNumber", number);
                parameters[3] = new SqlParameter("@LocationId1", loc1);
                parameters[4] = new SqlParameter("@LocationId2", loc2);

                parameters[5] = new SqlParameter("@LocationId3", loc3);
                parameters[6] = new SqlParameter("@Comments", comments);

                parameters[7] = new SqlParameter("@ContactLevelUnderstanding", understanding);
                parameters[8] = new SqlParameter("@Notification", notification);


                parameters[9] = new SqlParameter("@IsSaved", outParamSave);
                parameters[9].Direction = ParameterDirection.Output;

                parameters[10] = new SqlParameter("@IsContactDetailsAdded", isContactDetailsAdded);

                parameters[11] = new SqlParameter("@Email", email);
                parameters[12] = new SqlParameter("@Passowrd", pwd);
                parameters[13] = new SqlParameter("@LastName", LastName);

                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[9].Value == null ? 0 : Convert.ToInt32(parameters[9].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool SaveFeedback( string mobile, string importData)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {             

                string spName = "spSaveFeedback";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@MobileNumber", mobile);
                parameters[1] = new SqlParameter("@ImportData", importData);             
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;             
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool SaveContactSuggestions(int sourceid, int contactid, string category, string subcate, string microcat, string bussiName, bool iscitilevel, string busCont, string loc1, string loc2, string loc3, string comments, string importdata, string locationId4, string locationId5, string @locationId6, string contactComments, bool isAChain, string platForm, int city, int requestId, bool usedTagSuggestion)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {


                string spName = "spSaveContactSuggestions";
                SqlParameter[] parameters = new SqlParameter[23];
                parameters[0] = new SqlParameter("@SourceId", sourceid);
                parameters[1] = new SqlParameter("@ContactId", contactid);
                parameters[2] = new SqlParameter("@Category", category);
                parameters[3] = new SqlParameter("@SubCategory", subcate);
                parameters[4] = new SqlParameter("@Microcategory", microcat);
                parameters[5] = new SqlParameter("@CitiLevelBusiness", iscitilevel);
                parameters[6] = new SqlParameter("@BusinessName", bussiName);
                parameters[7] = new SqlParameter("@BusinessContact", busCont);
                parameters[8] = new SqlParameter("@LocationId1", loc1);
                parameters[9] = new SqlParameter("@LocationId2", loc2);
                parameters[10] = new SqlParameter("@LocationId3", loc3);
                parameters[11] = new SqlParameter("@Comments", comments);
                parameters[12] = new SqlParameter("@ImportData", importdata);
                parameters[13] = new SqlParameter("@LocationId4", locationId4);
                parameters[14] = new SqlParameter("@LocationId5", locationId5);
                parameters[15] = new SqlParameter("@LocationId6", locationId6);
                parameters[16] = new SqlParameter("@ContactComments", contactComments);
                parameters[17] = new SqlParameter("@IsAChain", isAChain);
                parameters[18] = new SqlParameter("@IsSaved", outParamSave);
                parameters[18].Direction = ParameterDirection.Output;
                parameters[19] = new SqlParameter("@Platform", platForm);
                parameters[20] = new SqlParameter("@City", city);
                parameters[21] = new SqlParameter("@RequestID", requestId);
                parameters[22] = new SqlParameter("@UsedTagSuggetion", usedTagSuggestion);

                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[18].Value == null ? 0 : Convert.ToInt32(parameters[18].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public bool SaveUserBusinessDetails(string userXmlData, int userBusinessId)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spUserBusiness_UserBusinessSave";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ImportData", userXmlData);
                parameters[1] = new SqlParameter("@ExistingBUID", userBusinessId);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public bool SaveMicroCategory(int subcate, string name, int addedby, int microid)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spSaveMicroCategory";
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@AddedBy", addedby);
                parameters[1] = new SqlParameter("@Name", name);
                parameters[2] = new SqlParameter("@SubCateId", subcate);
                parameters[3] = new SqlParameter("@MicroId", microid);
                parameters[4] = new SqlParameter("@IsSaved", outParamSave);
                parameters[4].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[4].Value == null ? 0 : Convert.ToInt32(parameters[4].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public bool SaveSource(string contactName, string ContactNumber, string MyPassword, string FriendsPassword, bool IsInterns)
        {

            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spSaveSource";
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@ContactName", contactName);
                parameters[1] = new SqlParameter("@ContactNumber", ContactNumber);
                parameters[2] = new SqlParameter("@MyPassword", MyPassword);
                parameters[3] = new SqlParameter("@FriendsPassword", FriendsPassword);
                parameters[4] = new SqlParameter("@IsInterns", IsInterns);
                parameters[5] = new SqlParameter("@IsSaved", outParamSave);
                parameters[5].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[5].Value == null ? 0 : Convert.ToInt32(parameters[5].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool SaveLocation(string suburb, string location, int City)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spSaveLoction";
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@Suburb", suburb);
                parameters[1] = new SqlParameter("@LocationName", location);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                parameters[3] = new SqlParameter("@City", City);
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public DataTable GetSuburb(int city)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spLoctionSuburb";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CityId", city);
                parameters[1] = new SqlParameter("@AreaShortCode", DBNull.Value);
                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public bool SaveRequestSuggestion(int uid, int catid, int subcatid, int microcatid, string location, int cityid, string comments, int contactid, int platform)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spSaveRequestSuggestion";
                SqlParameter[] parameters = new SqlParameter[10];
                parameters[0] = new SqlParameter("@UID", uid);
                parameters[1] = new SqlParameter("@CategoryId", catid);
                parameters[3] = new SqlParameter("@SubCategoryId", subcatid);
                parameters[4] = new SqlParameter("@MicrocategoryId", microcatid);
                parameters[5] = new SqlParameter("@Location", location);
                parameters[6] = new SqlParameter("@CityId", cityid);
                parameters[7] = new SqlParameter("@Comments", comments);
                parameters[8] = new SqlParameter("@ContactId", contactid);
                parameters[9] = new SqlParameter("@Platform", platform);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public DataTable GetDeviceDetails(int contactId)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetDeviceDetails";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ContactId", contactId);
                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetCity()
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetCity";              
                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetContactBySourceContact(string contactNumber)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetContactBySourceContact";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ContactNumber", contactNumber);
                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public bool UpdateNotificationTimeSent(string deviceUIDList)
        {
            bool result = true;

            try
            {

                string spName = "spUpdateNotificationTimeSent";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@DeviceUID", deviceUIDList);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
            }
            catch (Exception ex)
            {
                result = false;
                // throw ex;
            }
            return result;
        }
        public bool SaveNotificationForWebSend(int ? uid, int ? subcatid, int ? microcatid, int contactid, int deviceUID , string description, string NotificationType,bool sent, string NotificationTitle,int ? location, string NotificationPhoto, string RedirectTo, int sentBy)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
  
	



                string spName = "spSaveNotificationForWebSend";
                SqlParameter[] parameters = new SqlParameter[22];
                parameters[0] = new SqlParameter("@UID", uid);
                parameters[1] = new SqlParameter("DeviceUID", deviceUID);
                parameters[3] = new SqlParameter("@SubCategoryId", subcatid);
                parameters[4] = new SqlParameter("@MicrocategoryId", microcatid);
                parameters[5] = new SqlParameter("@Location", location);
                parameters[6] = new SqlParameter("@Text", description);
                parameters[7] = new SqlParameter("@NotificationType", NotificationType);
                parameters[8] = new SqlParameter("@ContactId", contactid);
                parameters[9] = new SqlParameter("@NotificationTitle", NotificationTitle);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;

                parameters[10] = new SqlParameter("@Sent", sent);
                parameters[11] = new SqlParameter("@ScheduleTIme", DBNull.Value);
                parameters[12] = new SqlParameter("@ConfirmTime", DBNull.Value);
                parameters[13] = new SqlParameter("@FeedbackResponse", DBNull.Value);

                parameters[14] = new SqlParameter("@FBTime", DBNull.Value);
                parameters[15] = new SqlParameter("@NotificationPhoto", NotificationPhoto);
                parameters[16] = new SqlParameter("@TimeSent", DBNull.Value);

                parameters[17] = new SqlParameter("@DeviceID", DBNull.Value);
                parameters[18] = new SqlParameter("@Type", DBNull.Value);
                parameters[19] = new SqlParameter("@Token", DBNull.Value);
                parameters[20] = new SqlParameter("@RedirectTo", RedirectTo);
                parameters[21] = new SqlParameter("@SentNotificationBy", sentBy);
                

                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
            
        }
        public DataSet GetSourcesToken()
        {
            DataSet dtuserDtails = new DataSet();

            try
            {
                string spName = "spGetSourcesToken";
                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataSet GetSourcesTokenByContactId(int contactId)
        {
            DataSet dtuserDtails = new DataSet();
            try
            {
                string spName = "spGetSourcesTokenByContactId";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ContactId", contactId);
                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataSet GetSuggestionList(int? catId, int? subCatId, int? sugId, int? contactId, string token, int? sourceId, string businessName, bool? isLocal, string location, int? microcate,string microName, int? cityId, DateTime? startDate, DateTime? endDate, bool? isChain, bool? isValid, bool? isVerified)
        {


            DataSet dtuserDtails = new DataSet();

            try
            {

                SqlParameter[] parameters = new SqlParameter[17];
                parameters[0] = new SqlParameter("@CatId", catId);
                parameters[1] = new SqlParameter("@SubCatId", subCatId);
                parameters[2] = new SqlParameter("@ContactId", contactId);
                parameters[3] = new SqlParameter("@SugId", sugId);
                if (!string.IsNullOrEmpty(token))
                {
                    parameters[4] = new SqlParameter("@Token", token);
                }
                else
                {
                    parameters[4] = new SqlParameter("@Token", DBNull.Value);
                }
                parameters[5] = new SqlParameter("@SourceId", sourceId);

                if (businessName == "")
                {
                    parameters[6] = new SqlParameter("@BusinessName", DBNull.Value);
                }
                else
                {
                    parameters[6] = new SqlParameter("@BusinessName", businessName);
                }
                if (location == "")
                {
                    parameters[7] = new SqlParameter("@Location", DBNull.Value);
                }
                else
                {

                    parameters[7] = new SqlParameter("@Location", location);
                }
                parameters[8] = new SqlParameter("@IsLocal", isLocal);
                parameters[9] = new SqlParameter("@Microcategory", microcate);

            

                // parameters[13] = new SqlParameter("@MCName", microName);

                if (microName == "")
                {
                    parameters[10] = new SqlParameter("@MCName", DBNull.Value);
                }
                else
                {

                    parameters[10] = new SqlParameter("@MCName", microName);
                }
                parameters[11] = new SqlParameter("@CityId", cityId);

                if (startDate == null)
                {
                    parameters[12] = new SqlParameter("@StartDate", DBNull.Value);
                }
                else
                {

                    parameters[12] = new SqlParameter("@StartDate", startDate);
                }
                if (endDate == null)
                {
                    parameters[13] = new SqlParameter("@EndDate", DBNull.Value);
                }
                else
                {

                    parameters[13] = new SqlParameter("@EndDate", endDate);
                }
                parameters[14] = new SqlParameter("@IsAChain", isChain);
                parameters[15] = new SqlParameter("@IsValid", isValid);
                parameters[16] = new SqlParameter("@IsVerified", isVerified);               
                string spName = "spGetSuggestionList";
                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
               // returnTotalRows = parameters[10].Value == null ? 0 : Convert.ToInt32(parameters[10].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public bool VerifySuggesion(int uid)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spVerifyContactSuggestions";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@SuggId", uid);
                parameters[1] = new SqlParameter("@IsSaved", outParamSave);
                parameters[1].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[1].Value == null ? 0 : Convert.ToInt32(parameters[1].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdateContactSuggestions(int suggId, int sourceid, int contactid, string category, string subcate, string microcat, string bussiName, bool iscitilevel, string busCont, string loc1, string loc2, string loc3, string comments, string importdata, string locationId4, string locationId5, string @locationId6, string contactComments, bool isAChain, string platForm, int? city)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {



                string spName = "spUpdateContactSuggestions";
                SqlParameter[] parameters = new SqlParameter[22];
                parameters[0] = new SqlParameter("@SourceId", sourceid);
                parameters[1] = new SqlParameter("@ContactId", contactid);
                parameters[2] = new SqlParameter("@Category", category);
                parameters[3] = new SqlParameter("@SubCategory", subcate);
                parameters[4] = new SqlParameter("@Microcategory", microcat);
                parameters[5] = new SqlParameter("@CitiLevelBusiness", iscitilevel);
                parameters[6] = new SqlParameter("@BusinessName", bussiName);
                parameters[7] = new SqlParameter("@BusinessContact", busCont);
                parameters[8] = new SqlParameter("@LocationId1", loc1);
                parameters[9] = new SqlParameter("@LocationId2", loc2);
                parameters[10] = new SqlParameter("@LocationId3", loc3);
                parameters[11] = new SqlParameter("@Comments", comments);
                parameters[12] = new SqlParameter("@ImportData", importdata);
                parameters[13] = new SqlParameter("@LocationId4", locationId4);
                parameters[14] = new SqlParameter("@LocationId5", locationId5);
                parameters[15] = new SqlParameter("@LocationId6", locationId6);
                parameters[16] = new SqlParameter("@ContactComments", contactComments);
                parameters[17] = new SqlParameter("@IsAChain", isAChain);
                parameters[18] = new SqlParameter("@IsSaved", outParamSave);
                parameters[18].Direction = ParameterDirection.Output;
                parameters[19] = new SqlParameter("@SuggId", suggId);
                parameters[20] = new SqlParameter("@Platform", platForm);
                parameters[21] = new SqlParameter("@City", city);
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[18].Value == null ? 0 : Convert.ToInt32(parameters[18].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool DeleteSuggesion(int uid, string reasonForChange)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spDeleteContactSuggestions";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@SuggId", uid);
                parameters[1] = new SqlParameter("@IsSaved", outParamSave);
                parameters[1].Direction = ParameterDirection.Output;
                parameters[2] = new SqlParameter("@ReasonForChange", reasonForChange);
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[1].Value == null ? 0 : Convert.ToInt32(parameters[1].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}