using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGetDataWebSite
{
    public class Crud
    {
        ConnectDB db = new ConnectDB();
        public int InsertPlayers(Obj_Players obj ) {
            string result = string.Empty;
            try
            {
                InsertPlayer(obj);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return 0;
        }
        public int InsertPlayer(Obj_Players objs)
        {
            Obj_Player obj = new Obj_Player();
            obj = objs.Player;
            int result=0;
            try
            {
                db.TableName = "Player";
                db.TableId = obj.Id;
                if (obj != null)
                {
                    List<SqlParameter> lstpr = FC_Convert.ClassToSqlParameter(obj);
                    result =FC_Convert.ParseInt(db.Save(lstpr));
                    if (result > 0) {
                        InsertPlayerDetail(objs.PlayerDetail,result);
                        InsertIndexPosition(objs.LstIndexPosition, result);
                        InsertIndexHidden(objs.LstIndexHidden, result);
                    }
                }               
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }
        public int InsertPlayerDetail(Obj_PlayerDetail obj,int playerId)
        {            
            int result = 0;
            try
            {
                db.TableName = "PlayerDetail";
                db.TableId = obj.Id;
                if (obj != null)
                {
                    obj.PlayerID = playerId;
                    List<SqlParameter> lstpr = FC_Convert.ClassToSqlParameter(obj);
                    result = FC_Convert.ParseInt(db.Save(lstpr));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public int InsertIndexPosition(List<Obj_IndexPosition> lst, int playerId)
        {
            int result = 0;
            try
            {
                db.TableName = "IndexPosition";
                foreach (var obj in lst)
                {
                    db.TableId = obj.Id;
                    if (obj != null)
                    {
                        obj.PlayerId = playerId;
                        List<SqlParameter> lstpr = FC_Convert.ClassToSqlParameter(obj);
                        result = FC_Convert.ParseInt(db.Save(lstpr));
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public int InsertIndexHidden(List<Obj_IndexHidden> lst, int playerId)
        {
            int result = 0;
            try
            {
                db.TableName = "IndexHidden";
                foreach (var obj in lst)
                {
                    db.TableId = obj.Id;
                    if (obj != null)
                    {
                        obj.PlayerId = playerId;
                        List<SqlParameter> lstpr = FC_Convert.ClassToSqlParameter(obj);
                        result = FC_Convert.ParseInt(db.Save(lstpr));
                    }
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
