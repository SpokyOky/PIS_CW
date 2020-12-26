using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Logics
{
    public class UserLogic
    {
        private Database context;

        public UserLogic(Database _context)
        {
            context = _context;
        }

        public void CreateOrUpdate(User user)
        {
            User tempUser = user.Id.HasValue ? null : new User();

            if (user.Id.HasValue)
            {
                tempUser = context.Users.FirstOrDefault(rec => rec.Id == user.Id);
            }

            if (user.Id.HasValue)
            {
                if (tempUser == null)
                {
                    throw new Exception("Элемент не найден");
                }

                tempUser.Id = user.Id;
                tempUser.Login = user.Login;
                tempUser.Password = user.Password;
                tempUser.Role = user.Role;
                tempUser.Name = user.Name;
            }
            else
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
        }

        public void Delete(User user)
        {
            User element = context.Users.FirstOrDefault(rec => rec.Id == user.Id.Value);

            if (element != null)
            {
                context.Users.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public List<User> Read(User user)
        {
            List<User> result = new List<User>();

            if (user != null)
            {
                result.AddRange(context.Users
                    .Where(rec => (rec.Id == user.Id) || ((rec.Login == user.Login || rec.Name == user.Name)
                    && (user.Password != null || rec.Password == user.Password)) || (rec.Role == user.Role))
                    .Select(rec => rec));
            }
            else
            {
                result.AddRange(context.Users);
            }

            return result;
        }
    }
}
