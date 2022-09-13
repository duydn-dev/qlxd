using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Const
{
    public static class CommonConstant
    {
        public enum DocumentState
        {
            NotHad = 0,
            Had = 1,
            Edited = 2
        }
        public enum UserPositionStatusEnum
        {
            NoActive,
            Active
        }
        public enum InputStatusEnum
        {
            Enough = 1,
            NotEnough = 2
        }
        public enum LoaiDoiTuongEnum
        {
            Tnpp = 1,
            Dmkd = 2,
            Dmsx = 3,
            Dlbl = 4,
            Ch = 5,
            Tdl = 6,
            Ncbl = 7,
        }
        public enum ApiNumberEnum
        {
            Api1 = 1,
            Api2 = 2,
            Api3 = 3,
            Api4 = 4,
            Api5 = 5,
            Api6 = 6,
            Api7 = 7,
            Api8 = 8,
            Api9 = 9,
            Api10 = 10,
        }
        public enum NumberDayOfWeekEnum
        {
            Monday = 2,
            Tuesday = 3,
            Wednesday = 4,
            Thursday = 5,
            Friday = 6,
            Saturday = 7,
            Sunday = 8,
        }
        public enum LoaiSoHuuEnum
        {
            SoHuu = 0,
            DongSoHuu = 1,
        }
        public enum LoaiBanEnum
        {
            TrongNuoc = 0,
            XuatKhau = 1,
        }
        public enum LoaiNhapEnum
        {
            TrongNuoc = 0,
            XuatKhau = 1,
        }
        public enum VungMienEnum
        {
            Bac = 0,
            Trung = 1,
            Nam = 2,
        }
        public enum DonViTinhEnum
        {
            M3 = 0,
            Tan = 1,
            Lit = 2,
            Thung = 3,
        }
        public const string AppName = "BoCongThuongVietNam_QuanLyXanDau";
        public const string SecertKey = "4nsNszIrKkZleRURl70i";
        public const string Issuer = "HSHXPnx8XTPvaFNrIMyv";
        public const string PrivateKey = "MIIEogIBAAKCAQEAoJJC8VrC3UOXEDutjgjmY6mY2NZv5oe77tSxVpEJs4l7GReIMormWAtKXirgpSq2t+IqOFYSpBFG2eF3+w357aTS8opGYA8SykO2jJkZT6gBcNpkv1ZgsjIoWAELEnQ0ZWQNDylFD7AmkCQ82YzNm89vd6J2EMqADPIzjTAR+aAAF7vU75L4+xlEh4uOiWAeo15DPnxvJ8WFMyJw9Cv1AjxSqfiNKxAHgQCV/VCxUbak/2H8nzuY+VtUQaUfrrQ/cderYYHGOrWQre1JyhaFDxaHBg6AFgvv8jJooc6yXb6qtyXUyHRsJ5RnIixvX9iJG8ytafSe+Alk1/6GyUWAjQIDAQABAoIBAEpPBa3eO9nb4rf/djUKB1zN6s9Ghaig3icLxTnzixLMJ+yXyE7jmu/Z4cOsgiPNnZuBjdpecuf8mtZQEo9bi3Mf2QOnXCQuNZrnNT7hxyXm8SvB1ef6WGt+7M61RON4ZZsabzS5+5zj/ySrsIFZOPnHSKWLyKnCnPngyFyph1gGqPSjrkBP1SV5E+xeit/lmysOqpiHOsC+QqoFa2U3m7CCuaxd8ijTtvXNGPgX8zGV0/H9iiHeg6b1BeAe0zaS9w/MlcGDuqpf/a2ZD68ucfkaN5/LNoG4h7YLx6KBlPPfHHnGFObx6g9Sid31HQawchyMV76mG1tUvPEeDbSd40ECgYEAzm3ftray16N77wA7p+gZ/02zcLFZr+nnCMu6z+YpQmtRIaPma7k/4QGNoyUf35I3rcGlYK0qETEgXFprXwBdHcM/gvKSjc46jl2WdzdSLY9J21YFh7LW671vqsMUEmyljtItD1Cg3geOEZ7ok4gXUfB7QtYAWlkjFUSEiNwswR0CgYEAxyFMvFWsuNwf0QncjB2tpdl6zCSj+QksW2XtTVEj0emGZhTjOIpZ+RcyViPausJh7NF/KMsO6Mvc1HWQlHtW+WnBJEEa/iGFXYuLfZTIgHUncekWafM9cfK1I0q35QAUdIX//9gcnCbSrMk9O58wbsbrcwtKfYzLHMW3THY6kjECgYBHEbbare3GpedOMnNXbGY+6l5j5vsswEelVJa9as07q7zj6wyye7XNXn6H84kUrL90l926+gMophQxF4Qt0e89BvJ/v8nK/nxUdU4PP0GQo+tWkzgWLvElejPOw5jOew9Iq32N70FjD5DO6jnCSTFWKYgWS50VfejBMrS6dasG1QKBgCyQlEbV+bjJcslqppQpFDK40FWdoaDbX7T4w7n4/cY9uBlidVOzCt3HRjmm/o2rcRT58bZkc2rALSTTRs8kJ1vcQiBl/a+AOwOrdkVdpd2x2mkQ3DZL/KVixw0l7K/wa6OEVb2hVQ3RVnba9rxALSutkwYFMd+VQeOnkBUppIQRAoGAOowEp8xWiccXitCsn5eMSS6izJJqPT8v5e4tcweoGv5TbqJDbP4H0uNLUnImujnnfwpDEJbByOCnBtexQjNTV63L0ipGwayCn9jysXsJwWC+Pf+dS3OONYl1aOeqHmGuFn3bymUR5XxIYhDI18Rj+In+Xkt9iJf+glg6AeAK9mU=";
        public const string PublicKey = "MIIBCgKCAQEAoJJC8VrC3UOXEDutjgjmY6mY2NZv5oe77tSxVpEJs4l7GReIMormWAtKXirgpSq2t+IqOFYSpBFG2eF3+w357aTS8opGYA8SykO2jJkZT6gBcNpkv1ZgsjIoWAELEnQ0ZWQNDylFD7AmkCQ82YzNm89vd6J2EMqADPIzjTAR+aAAF7vU75L4+xlEh4uOiWAeo15DPnxvJ8WFMyJw9Cv1AjxSqfiNKxAHgQCV/VCxUbak/2H8nzuY+VtUQaUfrrQ/cderYYHGOrWQre1JyhaFDxaHBg6AFgvv8jJooc6yXb6qtyXUyHRsJ5RnIixvX9iJG8ytafSe+Alk1/6GyUWAjQIDAQAB";
    }
    public static class UserStatus
    {
        public const int Locked = 0;
        public const int Working = 1;
    }
    public static class MeetRoomStatus
    {
        public const int Offline = 0;
        public const int Online = 1;
    }
    public static class ProjectState
    {
        public const int Excuting = 0;
        public const int Excuted = 1;
    }
    public static class PositionState
    {
        public const int Deactived = 0;
        public const int Active = 1;
    }
    public static class LocalityType
    {
        public const int Tinh = 1;
        public const int Huyen = 2;
        public const int Xa = 3;
        public const int So = 4;
    }
    public static class DataFormatUnitType
    {
        public const int Point = 1;
        public const int Percent = 2;
    }
    public static class ApprovedStatus
    {
        public const int NotInput = -1;
        public const int Inputed = 0;
        public const int Approve = 1;
        public const int Reject = 2;
    }
    public static class TrangThai
    {
        public const int NotUse = 0;
        public const int Use = 1;
    }
}
