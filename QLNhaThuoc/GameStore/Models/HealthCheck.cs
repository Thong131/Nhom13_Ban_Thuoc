using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Models
{
	public class HealthCheck
	{
		public string Gender { get; set; } // Giới tính
		public double Height { get; set; } // Chiều cao (cm)
		public double Weight { get; set; } // Cân nặng (kg)

		public double BMI => CalculateBMI(); // Chỉ số BMI

		private double CalculateBMI()
		{
			if (Height <= 0) return 0; // Kiểm tra chiều cao hợp lệ
			return Weight / ((Height / 100) * (Height / 100)); // Công thức tính BMI
		}
	}
}